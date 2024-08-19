using System.Text;

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
            ImportMap(pitchList);
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
                Name = d.Name,
                Pitch = d.INote,
                OutPitch = d.ONote
            };

            Items.Add(item);
        }

        if (basedOnONote && Items.GroupBy(x => x.OutPitch).Where(x => x.Count() > 1).Any())
        {
            throw new Exception("Duplicate ONote");
        }
    }

    private static void ImportMap(PitchList drum)
    {
        Name = drum.Title;

        foreach (var d in drum.Items)
        {
            MapItem item = new()
            {
                Pitch = d.Pitch,
                Name = d.Name,
                OutPitch = d.Pitch
            };

            Items.Add(item);
        }
    }

    private static void ImportMap(string file)
    {
        string[] data = File.ReadAllLines(file);

        for (int i = 1; i < data.Length; i++)
        {
            var col = data[i].Split(',');

            if (col.Length != 8)
            {
                throw new Exception();
            }

            if (string.IsNullOrWhiteSpace(col[5])) continue;

            MapItem item = new();
            item.Pitch = int.Parse(col[1]);
            item.Name = $"{col[5]}";
            item.OutPitch = int.Parse(col[3]);
            item.Duplicate = $"{col[4]}";
            item.Check = $"{col[5]}";

            Items.Add(item);
        }
    }

    private static void ToPitchList(string file, bool onote)
    {
        StringBuilder builder = new();

        builder.AppendLine($"<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.AppendLine($"<Music.PitchNameList title=\"{EscXML(Map.Name)}\">");

        foreach (var item in Map.Items)
        {
            if (item.Pitch != item.OutPitch)
            {
                builder.AppendLine($"\t<!-- In Pitch = {item.Pitch}, Out Pitch = {item.OutPitch} -->");
            }
            builder.AppendLine($"\t<Music.PitchName pitch=\"{(onote ? item.OutPitch : item.Pitch)}\" name=\"{EscXML(item.Name)}\"/>");
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
            if (Items.Where(x => x.Pitch == pitch).FirstOrDefault() is MapItem item)
            {
                cubase.Map.Items[pitch].Name = item.Name;
                cubase.Map.Items[pitch].ONote = item.OutPitch;
                cubase.Map.Items[pitch].DisplayNote = int.Parse($"0{item.Pitch}");
            }
        });

        int i = 1;
        Items.ForEach(item =>
        {
            odr[item.Pitch] = i;
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

        builder.AppendLine(string.Join(",", ["Order", "Pitch", "Note", "Out Pitch", "Out Note", "Name", "Duplicate", "Check"]));

        int order = 0;
        if (csvorder)
        {
            Enumerable.Range(0, 128).ToList().ForEach(pitch =>
            {
                MapItem? item = Items.FirstOrDefault(x => x.Pitch == pitch);

                var dup = Items.Where(x => x.OutPitch == item?.OutPitch).Count() > 1 ? "*" : string.Empty;

                if (item != null)
                {
                    builder.Append($"{order}");
                    builder.Append($",{pitch}");
                    builder.Append($",{Pitch.NoteName((byte)item.Pitch)}");
                    builder.Append($",{item.OutPitch}");
                    builder.Append($",{Pitch.NoteName((byte)item.OutPitch)}");
                    builder.Append($",{item.Name}");
                    builder.Append($",{dup}");
                    builder.AppendLine($",");
                }
                else
                {
                    builder.AppendLine($"{order},{order},{Pitch.NoteName((byte)order)},{order},{Pitch.NoteName((byte)order)},,");
                }
                order++;
            });
        }
        else
        {
            Items.ForEach(item =>
            {
                var dup = Items.Where(x => x.OutPitch == item.OutPitch).Count() > 1 ? "*" : string.Empty;

                builder.Append($"{order}");
                builder.Append($",{item.Pitch}");
                builder.Append($",{Pitch.NoteName((byte)item.Pitch)}");
                builder.Append($",{item.OutPitch}");
                builder.Append($",{Pitch.NoteName((byte)item.OutPitch)}");
                builder.Append($",{item.Name}");
                builder.Append($",{dup}");
                builder.AppendLine($",");
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
