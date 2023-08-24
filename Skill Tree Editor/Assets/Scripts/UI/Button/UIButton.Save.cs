using System.Xml.Linq;
public partial class UIButton 
{
    public XElement Export()
    {
        XElement data = new XElement("Skill");
        data.Add(new XElement("X", _pos.x));
        data.Add(new XElement("Y", _pos.y));

        data = _data.Export(data);
        data = _connectionData.Export(data);

        return data;
    }
}
