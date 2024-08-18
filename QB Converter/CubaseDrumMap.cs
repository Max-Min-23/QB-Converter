using System.Xml.Linq;
using System.Xml.XPath;

namespace Cubase;

public class CubaseDrumMap
{
    #region ctor

    public CubaseDrumMap() { }

    #endregion

    #region Properties

    public string Name { get; set; } = string.Empty;

    public QuantizeElement Quantize { get; set; } = new();

    public MapElement Map { get; set; } = new();

    public List<int> Order { get; set; } = new();

    public OutputDevicesElement OutputDevices { get; set; } = new();

    public int Flags { get; set; }

    #endregion

    #region Methods

    public static CubaseDrumMap Load(string filePath)
    {
        CubaseDrumMap drumMap;

        if (XDocument.Load(filePath) is not XDocument doc) throw new FileNotFoundException(filePath);

        if (doc.XPathSelectElement($"/DrumMap") is not XElement root) throw new ArgumentException(filePath);

        IEnumerable<XElement> GetItems(string key) => root.XPathSelectElements($"*[@name='{key}']/item");

        drumMap = new();

        drumMap.Name = root.XPathSelectElement($"*[@name='{nameof(Name)}']")?.Attribute("value")?.Value ?? string.Empty;

        drumMap.Quantize.Load(GetItems(nameof(Quantize)));

        drumMap.Map.Load(GetItems((nameof(Map))));

        drumMap.Order = GetItems((nameof(Order))).Select(elm => int.Parse(elm.Attribute("value")?.Value ?? "0")).ToList();

        drumMap.OutputDevices.Load(GetItems(nameof(OutputDevices)));

        drumMap.Flags = int.Parse(root.XPathSelectElement($"*[@name='{nameof(Flags)}']")?.Attribute("value")?.Value ?? "0");

        return drumMap;
    }

    public void Save(string filePath)
    {
        XDocument doc = new XDocument(new XDeclaration(version: "1.0", encoding: "utf-8", null));

        XElement root = new XElement(nameof(CubaseDrumMap));

        doc.Add(root);

        root.Add(new XElement("string", [new XAttribute("name", "Name"), new XAttribute("value", Name), new XAttribute("wide", "true")]));

        root.Add(Quantize.ToElement());

        root.Add(Map.ToElement());

        XElement order = new XElement("list", [new XAttribute("name", nameof(Order)), new XAttribute("type", "int")]);
        Order.ForEach(item => order.Add(new XElement("item", new XAttribute("value", $"{item}"))));
        root.Add(order);

        root.Add(OutputDevices.ToElement());

        root.Add(new XElement("int", [new XAttribute("name", "Flags"), new XAttribute("value", Flags)]));

        doc.Save(filePath);
    }

    public void Initialize()
    {
        Name = "Default";

        Quantize.Items.Add(new QuantizeItem());

        Enumerable.Range(0, 128).ToList().ForEach(x =>
        {
            Map.Items.Add(new MapItem(string.Empty, x));
            Order.Add(x);
        });

        OutputDevices.Items.Add(new OutputDevicesItem());
    }

    #endregion
}

#region abstract class

public abstract class ListElement<T>
    where T : ListElementItem
{
    #region ctor

    public ListElement(IEnumerable<T> items)
    {
        Items = items.ToList();
    }

    #endregion

    #region Properties

    public List<T> Items { get; set; }

    #endregion

    #region Methods

    internal void Load(IEnumerable<XElement> elements)
    {
        elements.ToList().ForEach(element =>
        {
            if (Activator.CreateInstance(typeof(T)) is not T instance) throw new ArgumentException();
            instance.Load(element);
            Items.Add(instance);
        });
    }

    internal XElement ToElement()
    {
        XElement element =
            new(
                "list",
                [
                    new XAttribute("name", GetType().Name.Replace("Element", string.Empty)),
                    new XAttribute("type", "list"),
                ]
            );

        Items.ForEach(item => element.Add(item.ToElement()));

        return element;
    }

    #endregion
}

public abstract class ListElementItem
{
    #region Methods

    internal void Load(XElement element)
    {
        foreach (var prop in GetType().GetProperties())
        {
            if (element.XPathSelectElement($"*[@name='{prop.Name}']") is not XElement item) continue;

            object value = null!;

            if (prop.PropertyType == typeof(string))
                value = item.Attribute("value")?.Value ?? string.Empty;
            else if (prop.PropertyType == typeof(int))
                value = int.Parse(item.Attribute("value")?.Value ?? "0");
            else if (prop.PropertyType == typeof(float))
                value = float.Parse(item.Attribute("value")?.Value ?? "0");
            else
                continue;

            if (value == null) throw new ArgumentException();

            prop.SetValue(this, value);
        }
    }

    string getTypeName(Type type)
    {
        if (type == typeof(int)) return "int";
        if (type == typeof(string)) return "string";
        if (type == typeof(float)) return "float";
        throw new ArgumentException();
    }

    internal XElement ToElement()
    {
        XElement element = new XElement("item");

        foreach (var p in GetType().GetProperties())
            element.Add(
                new XElement(getTypeName(p.PropertyType),
                p.PropertyType == typeof(string) ?
                    new XAttribute[] {
                        new XAttribute("name", $"{p.Name}"),
                        new XAttribute("value", $"{p.GetValue(this)}"),
                        new XAttribute("wide", "true"),
                    }
                    :
                    [
                        new XAttribute("name", $"{p.Name}"),
                        new XAttribute("value", $"{p.GetValue(this)}")
                    ]
                )
            );

        return element;
    }

    #endregion
}

#endregion

#region Element class

public class QuantizeElement
    : ListElement<QuantizeItem>
{
    public QuantizeElement() : base(new List<QuantizeItem>()) { }
}

public class QuantizeItem
    : ListElementItem
{
    public int Grid { get; set; } = 4;
    public int Type { get; set; } = 0;
    public float Swing { get; set; } = 0;
    public int Legato { get; set; } = 50;
}

public class MapElement
    : ListElement<MapItem>
{
    public MapElement() : base(new List<MapItem>()) { }
}

public class MapItem
    : ListElementItem
{
    public MapItem()
    {

    }

    public MapItem(string name, int inote)
    {
        Name = name;
        INote = inote;
        ONote = inote;
        DisplayNote = inote;
    }

    public int INote { get; set; } = 0;
    public int ONote { get; set; } = 0;
    public int Channel { get; set; } = -1;
    public float Length { get; set; } = 200;
    public int Mute { get; set; } = 0;
    public int DisplayNote { get; set; } = 0;
    public int HeadSymbol { get; set; } = 0;
    public int Voice { get; set; } = 0;
    public int PortIndex { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public int QuantizeIndex { get; set; } = 0;
}

public class OutputDevicesElement
    : ListElement<OutputDevicesItem>
{
    public OutputDevicesElement() : base(new List<OutputDevicesItem>()) { }
}

public class OutputDevicesItem
    : ListElementItem
{
    public string DeviceName { get; set; } = "Default Device";
    public string PortName { get; set; } = "Default Port";
}

#endregion