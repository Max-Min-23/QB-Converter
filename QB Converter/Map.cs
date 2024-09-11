using System.Text;
using System.Xml;
using System.Xml.Linq;
using Cubase;

namespace QB_Converter;

public static class Map
{
    private static string FilePath = string.Empty;
    private static string Name { get; set; } = string.Empty;

    public static string Import(string file, FileExtension from, FileExtension to, bool isRaw)
    {
        StringBuilder sb = new();

        sb.AppendLine($"Convert Report {file} From:{from} To:{to} IsRow:{isRaw}");

        // check file
        if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
        {
            sb.AppendLine($"#Skip 'Directory'");
            goto labExit;
        }

        if (Path.GetExtension(file).Substring(1) != $"{from}")
        {
            sb.AppendLine($"#Skip 'Extension Unmatch'");
            goto labExit;
        }

        // proc Import
        List<MapItem>? items = null;

        try
        {
            items =
                from switch
                {
                    FileExtension.pitchlist => ImportPitchlist(file),
                    FileExtension.drm => ImportDrm(file),
                    FileExtension.csv => ImportCSV(file),
                    FileExtension.txt => ImportTxt(file),
                    _ => throw new NotImplementedException(),
                };
        }
        catch (Exception ex)
        {
            sb.AppendLine($"#Abend Import");
            sb.AppendLine(ex.Message);
            goto labExit;
        }

        if (items == null)
        {
            sb.AppendLine($"#Abend Import");
            sb.AppendLine("Empty Data");
            goto labExit;
        }

        // proc Export
        try
        {
            bool ret =
                to switch
                {
                    FileExtension.pitchlist => ToPitchList(items, file),
                    FileExtension.drm => ToDrm(items, file),
                    FileExtension.csv => ToCSV(items, file, isRaw),
                    FileExtension.txt => ToTxt(items, file),
                    _ => throw new NotImplementedException(),
                };
        }
        catch (Exception ex)
        {
            sb.AppendLine($"#Abend Export");
            sb.AppendLine(ex.Message);
            goto labExit;
        }

        sb.AppendLine($"Convert Compleat");

    labExit:
        sb.AppendLine(string.Empty);
        return sb.ToString();
    }

    private static List<MapItem>? ImportDrm(string file)
    {
        if (CubaseDrumMap.Load(file) is not CubaseDrumMap drum) throw new ArgumentException();

        var items = new List<MapItem>();

        Name = drum.Name;

        foreach (var d in drum.Order.Select(x => drum.Map.Items[x]))
        {
            MapItem item =
                new()
                {
                    PitchName = d.Name,
                    InPitch = d.INote,
                    OutPitch = d.ONote
                };
            items.Add(item);
        }

        return items;
    }

    private static List<MapItem>? ImportPitchlist(string file)
    {
        if (StudioOnePitchList.Load(file) is not PitchList drum) throw new ArgumentException();

        var items = new List<MapItem>();

        Name = drum.Title;

        foreach (var d in drum.Items)
        {
            MapItem item = new()
            {
                InPitch = d.Pitch,
                PitchName = d.Name,
                OutPitch = d.Pitch
            };
            items.Add(item);
        }

        return items;
    }

    private static List<MapItem>? ImportCSV(string file)
    {
        string[] data = File.ReadAllLines(file);

        var items = new List<MapItem>();

        Name = Path.GetFileNameWithoutExtension(file);

        for (int i = 0; i < data.Length; i++)
        {
            MapItem map = MapItem.CreateItem(data[i]);

            if (map.IsHeader()) continue;

            if (map.IsValid())
                items.Add(map);
            else
                throw new ArgumentException();
        }

        return items;
    }

    private static List<MapItem>? ImportTxt(string file)
    {
        string[] data = File.ReadAllLines(file);

        var items = new List<MapItem>();

        Name = Path.GetFileNameWithoutExtension(file);

        int count = 0;

        foreach (var line in data.Select(x => x.Trim()))
        {
            count++;

            if (line.StartsWith("#")) continue;
            if (line.Trim().Length == 0) continue;

            string[] spl = line.Replace('\t', ' ').Split(' ');

            if (!int.TryParse(spl[0], out int inpitch)) throw new ArgumentException($"Bad Format Line:{count}");
            string name = string.Join(" ", spl.Skip(1).ToArray());

            MapItem item = new()
            {
                InPitch = inpitch,
                PitchName = name,
                OutPitch = inpitch
            };
            items.Add(item);
        }

        return items;
    }

    private static bool ToPitchList(List<MapItem> items, string file)
    {
        if (items.GroupBy(x => x.OutPitch).Select(x => x.Count()).Where(x => x > 1).Any())
        {
            throw new ArgumentException($"Duplicate OutPitch exist.");
        }

        StringBuilder builder = new();

        builder.AppendLine($"<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.AppendLine($"<Music.PitchNameList title=\"{EscXML(Name)}\">");

        foreach (var item in items)
        {
            builder.AppendLine($"\t<Music.PitchName pitch=\"{item.OutPitch}\" name=\"{EscXML(item.PitchName)}\"/>");
        }

        builder.AppendLine($"</Music.PitchNameList>");

        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}.{FileExtension.pitchlist}"), builder.ToString());
        return true;
    }

    private static bool ToDrm(List<MapItem> items, string file)
    {
        var cubase = new Cubase.CubaseDrumMap();
        cubase.Initialize();

        cubase.Name = Name;

        Dictionary<int, int> odr = new();

        Enumerable.Range(0, 128).ToList().ForEach(pitch =>
        {
            odr.Add(pitch, 999);
            if (items.Where(x => x.InPitch == pitch).FirstOrDefault() is MapItem item)
            {
                cubase.Map.Items[pitch].Name = item.PitchName;
                cubase.Map.Items[pitch].ONote = item.OutPitch;
                cubase.Map.Items[pitch].DisplayNote = int.Parse($"0{item.InPitch}");
            }
        });

        int i = 1;
        items.ForEach(item =>
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
            string filename = Path.Combine(dir, $"{Path.GetFileNameWithoutExtension(file)}.{FileExtension.drm}");
            cubase.Save(filename);
        }
        return true;
    }

    private static bool ToCSV(List<MapItem> items, string file, bool isRaw)
    {
        StringBuilder sb = new();

        sb.AppendLine(MapItem.GetCSVHeader());

        int seq = 0;

        if (isRaw)
        {
            Enumerable.Range(0, 128).ToList().ForEach(pitch =>
            {
                MapItem? item = items.FirstOrDefault(x => x.InPitch == pitch);

                if (item != null)
                {
                    item.Order = seq;
                    item.Duplicate = items.Where(x => x.OutPitch == item.OutPitch).Count() > 1 ? "*" : string.Empty;
                    sb.AppendLine($"{item}");
                }
                else
                {
                    sb.AppendLine($"{new MapItem { Order = seq }}");
                }
                seq++;
            });
        }
        else
        {
            items.ForEach(item =>
            {
                item.Order = seq;
                item.Duplicate = items.Where(x => x.OutPitch == item.OutPitch).Count() > 1 ? "*" : string.Empty;
                sb.AppendLine($"{item}");
                seq++;
            });
        }

        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}.{FileExtension.csv}"), sb.ToString());
        return true;
    }

    private static bool ToTxt(List<MapItem> items, string file)
    {
        StringBuilder sb = new();

        int seq = 0;

        sb.AppendLine($"# {Name}");

        items
            .Where(item => !string.IsNullOrWhiteSpace(item.PitchName))
            .ToList()
            .ForEach(item =>
        {
            item.Order = seq;
            sb.AppendLine($"{item.InPitch}\t{item.PitchName}");
            seq++;
        });

        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}.{FileExtension.txt}"), sb.ToString());

        return true;
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
