using System.Xml.Linq;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public XElement OnSave()
    {
        XElement data = new XElement("Skills");
        foreach (var button in _buttons.Values)
        {
            if (!button.CanExport())
                continue;

            data.Add(button.Export());
        }
        return data;
    }
}
