using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace QB_Converter;

public enum ConvertType
{
    BasedOnINote,
    BasedOnONote,
    SOneToCubase,
    CubaseToCSV,
    SOneToCSV,
    CSVToSOne,
    CSVToCubase
}

public static class FileType
{
    public const string drm = ".drm";
    public const string pitchlist = ".pitchlist";
    public const string csv = ".csv";
}

public static class Map
{
    private static string FilePath = string.Empty;
    private static string Name { get; set; } = string.Empty;
    private static List<MapItem> Items = new();

    public static void Import(string file, ConvertType convertType, bool csvorder)
    {
        Items.Clear();

        string ext = Path.GetExtension(file).ToLower();

        if (ext != FileType.drm && ext != FileType.pitchlist & ext != FileType.csv)
        {
            return;
        }

        if (ext == FileType.drm && convertType != ConvertType.BasedOnINote && convertType != ConvertType.BasedOnONote && convertType != ConvertType.CubaseToCSV)
        {
            return;
        }
        else if (ext == FileType.pitchlist && convertType != ConvertType.SOneToCubase && convertType != ConvertType.SOneToCSV)
        {
            return;
        }
        else if (ext == FileType.csv && convertType != ConvertType.CSVToSOne && convertType != ConvertType.CSVToCubase)
        {
            return;
        }

        // Import Map
        if (ext == FileType.drm)
        {
            ImportMap(Cubase.CubaseDrumMap.Load(file), convertType == ConvertType.BasedOnONote, csvorder);
        }
        else if (ext == FileType.pitchlist)
        {
            if (StudioOnePitchList.Load(file) is not PitchList pitchList)
            {
                throw new Exception();
            }

            XDocument? xml = XDocument.Load(file);

            ImportMap(pitchList, xml);
        }
        else if (ext == FileType.csv)
        {
            ImportMap(file);
        }

        // Convert
        if (
            convertType == ConvertType.BasedOnINote ||
            convertType == ConvertType.BasedOnONote ||
            convertType == ConvertType.CSVToSOne
        )
        {
            ToPitchList(file, convertType == ConvertType.BasedOnONote);
        }
        else if (
            convertType == ConvertType.SOneToCubase ||
            convertType == ConvertType.CSVToCubase
        )
        {
            ToDrm(file);
        }
        else if (
            convertType == ConvertType.SOneToCSV ||
            convertType == ConvertType.CubaseToCSV
        )
        {
            ToCSV(file, csvorder);
        }

    }

    private static void ImportMap(Cubase.CubaseDrumMap drum, bool basedOnONote, bool csvorder)
    {
        Name = drum.Name;

        foreach (var d in drum.Order.Select(x => drum.Map.Items[x]))
        {
            MapItem item = new()
            {
                PitchName = d.Name,
                InPitch = d.INote,
                OutPitch = d.ONote
            };

            Items.Add(item);
        }

        if (basedOnONote && Items.GroupBy(x => x.OutPitch).Where(x => x.Count() > 1).Any())
        {
            throw new Exception("Duplicate ONote");
        }
    }

    private static void ImportMap(PitchList drum, XDocument xml)
    {
        Name = drum.Title;

        foreach (var d in drum.Items)
        {
            MapItem item = new()
            {
                InPitch = d.Pitch,
                PitchName = d.Name,
                OutPitch = d.Pitch
            };

            Items.Add(item);
        }

        if (xml.Element("Music.PitchNameList")?.Nodes().Where(x => x.NodeType == XmlNodeType.Comment).ToArray() is not XNode[] comments)
        {
            return;
        }

        foreach (var com in comments)
        {
            var text = com.ToString().Substring(4, com.ToString().Length - 7);
            var spl1 = text.Split(',').Select(x => x.Trim()).ToArray();
            if (spl1.Length == 2)
            {
                var spl2 = spl1[0].Split("=").Select(x => x.Trim()).ToArray();
                var spl3 = spl1[1].Split("=").Select(x => x.Trim()).ToArray();
                if (spl2.Length == 2 && spl3.Length == 2 && spl2[0] == "In Pitch" && spl3[0] == "Out Pitch")
                {
                    int inp = 0;
                    int outp = 0;
                    if (int.TryParse(spl2[1], out inp) && int.TryParse(spl3[1], out outp))
                    {
                        if (Items.Where(x => x.InPitch == inp).FirstOrDefault() is MapItem map)
                        {
                            map.OutPitch = outp;
                        }
                    }
                }
            }
        }
    }

    private static void ImportMap(string file)
    {
        string[] data = File.ReadAllLines(file);

        for (int i = 0; i < data.Length; i++)
        {
            MapItem map = MapItem.CreateItem(data[i]);

            if (map.IsHeader()) continue;

            if (map.IsValid())
                Items.Add(map);
            else
                throw new ArgumentException();
        }
    }

    private static void ToPitchList(string file, bool onote)
    {
        StringBuilder builder = new();

        builder.AppendLine($"<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.AppendLine($"<Music.PitchNameList title=\"{EscXML(Map.Name)}\">");

        foreach (var item in Map.Items)
        {
            builder.AppendLine($"\t<!-- In Pitch = {item.InPitch}, Out Pitch = {item.OutPitch} -->");
            builder.AppendLine($"\t<Music.PitchName pitch=\"{(onote ? item.OutPitch : item.InPitch)}\" name=\"{EscXML(item.PitchName)}\"/>");
        }

        builder.AppendLine($"</Music.PitchNameList>");

        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}.pitchlist"), builder.ToString());
    }

    private static void ToDrm(string file)
    {
        var cubase = new Cubase.CubaseDrumMap();
        cubase.Initialize();

        cubase.Name = Map.Name;

        Dictionary<int, int> odr = new();

        Enumerable.Range(0, 128).ToList().ForEach(pitch =>
        {
            odr.Add(pitch, 999);
            if (Items.Where(x => x.InPitch == pitch).FirstOrDefault() is MapItem item)
            {
                cubase.Map.Items[pitch].Name = item.PitchName;
                cubase.Map.Items[pitch].ONote = item.OutPitch;
                cubase.Map.Items[pitch].DisplayNote = int.Parse($"0{item.InPitch}");
            }
        });

        int i = 1;
        Items.ForEach(item =>
        {
            odr[item.InPitch] = i;
            i++;
        });

        cubase.Order.Clear();

        cubase.Order.AddRange(
            odr
            .OrderBy(x => x.Value)
            .ThenBy(x => x.Key)
            .Select(x => x.Key)
            .ToArray()
        );

        if (Path.GetDirectoryName(file) is string dir)
        {
            string filename = Path.Combine(dir, $"{Path.GetFileNameWithoutExtension(file)}.drm");
            cubase.Save(filename);
        }
    }

    private static void ToCSV(string file, bool csvorder)
    {
        StringBuilder builder = new();

        builder.AppendLine(MapItem.GetCSVHeader());

        int order = 0;
        if (csvorder)
        {
            Enumerable.Range(0, 128).ToList().ForEach(pitch =>
            {
                MapItem? item = Items.FirstOrDefault(x => x.InPitch == pitch);

                var dup = Items.Where(x => x.OutPitch == item?.OutPitch).Count() > 1 ? "*" : string.Empty;

                if (item != null)
                {
                    item.Order = order;
                    builder.AppendLine($"{item}");
                }
                else
                {
                    builder.AppendLine($"{new MapItem { Order = order }}");
                }
                order++;
            });
        }
        else
        {
            Items.ForEach(item =>
            {
                item.Order = order;
                var dup = Items.Where(x => x.OutPitch == item.OutPitch).Count() > 1 ? "*" : string.Empty;
                builder.AppendLine($"{item}");
                order++;
            });
        }

        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}_{Path.GetExtension(file).Replace(".", "")}.csv"), builder.ToString());
    }

    private static string EscXML(string str)
    {
        return
            str.
                Replace("&", "&amp;").
                Replace("\"", "&quot;").
                Replace("'", "&apos;").
                Replace("<", "&lt;").
                Replace(">", "&gt;");
    }
}
