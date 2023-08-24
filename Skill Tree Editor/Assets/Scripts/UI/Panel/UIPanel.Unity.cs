using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UIPanel : MonoBehaviour
{
    [SerializeField] private Button _save, _load, _connections;
    [SerializeField] private TMP_Text _connectionsText;
    public void Awake()
    {
        _save.onClick.AddListener(OnSave);
        _load.onClick.AddListener(OnLoad);
        _connections.onClick.AddListener(OnConnections);

        UnityEngine.Windows.Directory.CreateDirectory(Application.streamingAssetsPath);
    }
}
