using System.ComponentModel;
using System.Reflection;

namespace QB_Converter;

public class MapItem
{
    public int Order { get; set; }
    public int InPitch { get; set; }
    public string InNote => InPitch.NoteName();
    public int OutPitch { get; set; }
    public string OutNote => OutPitch.NoteName();
    public string PitchName { get; set; } = string.Empty;
    public string Duplicate { get; set; } = string.Empty;
    public string Check { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;

    protected bool _IsHeader = false;
    protected bool _Valid = false;

    private IEnumerable<PropertyInfo> MyProperties => GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public static MapItem CreateItem(string line)
    {
        var spl = line.Split(',');

        var result = new MapItem();

        if (GetCSVHeader().Split(',').ToArray().SequenceEqual(spl.ToArray()))
        {
            result._IsHeader = true;
            return result;
        }

        foreach (var p in result.MyProperties.Select((x, i) => new { idx = i, x }))
        {
            if (p.idx > spl.Length - 1) continue;

            if (!p.x.CanWrite) continue;

            if (TypeDescriptor.GetConverter(p.x.PropertyType) is TypeConverter converter)
            {
                p.x.SetValue(result, converter.ConvertFrom(spl[p.idx]));
            }
        }

        result._Valid = true;
        return result;
    }

    public bool IsHeader() => _IsHeader;

    public bool IsValid() => _Valid;

    public static string GetCSVHeader()
    {
        return string.Join(",", new MapItem().MyProperties.Select(x => x.Name).ToArray());
    }

    public override string ToString()
    {
        return string.Join(",", MyProperties.Select(x => x.GetValue(this)).ToArray());
    }
}
