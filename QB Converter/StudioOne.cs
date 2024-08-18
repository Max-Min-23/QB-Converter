using System.Xml.Serialization;

namespace QB_Converter;

[Serializable()]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public partial class PitchName
{
    [XmlAttribute("pitch")]
    public byte Pitch { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("scorePitch")]
    public string ScorePitch { get; set; } = string.Empty;

    [XmlAttribute("notehead")]
    public string Notehead { get; set; } = string.Empty;

    [XmlAttribute("technique")]
    public string Technique { get; set; } = string.Empty;

    [XmlAttribute("flags")]
    public string Flags { get; set; } = string.Empty;
}

[Serializable()]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot("Music.PitchNameList", Namespace = "", IsNullable = false)]
public partial class PitchList
{
    [XmlElement("Music.PitchName")]
    public List<PitchName> Items { get; set; } = new();

    [XmlAttribute("title")]
    public string Title { get; set; } = string.Empty;
}

public static class StudioOnePitchList
{
    public static PitchList? LoadPitchList(string filepath)
    {
        var xml = new XmlSerializer(typeof(PitchList));
        using (StreamReader sr = new(filepath))
        {
            if (xml.Deserialize(sr) is PitchList list)
                return list;
            else
                return default;
        }
    }
}