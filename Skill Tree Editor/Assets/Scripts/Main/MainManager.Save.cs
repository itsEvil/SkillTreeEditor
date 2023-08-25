using System.Xml.Linq;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    [Tooltip("Cooldown between each save in seconds")]
    [SerializeField]
    private float _saveCooldown = 60f;
    private float _timeUntilSave;
    public XElement OnSave()
    {
        XElement data = new("Skills");
        foreach (var button in _buttons.Values)
        {
            if (!button.CanExport())
                continue;

            data.Add(button.Export());
        }
        return data;
    }
}
