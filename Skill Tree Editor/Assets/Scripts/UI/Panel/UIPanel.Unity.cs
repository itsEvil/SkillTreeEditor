using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UIPanel : MonoBehaviour
{
    [SerializeField] private Button _save, _load, _connections, _removal;
    [SerializeField] private TMP_Text _connectionsText, _removalText;
    [SerializeField] private Image _connectionsImage, _removalImage;
    public void Awake()
    {
        Instance = this;

        _save.onClick.AddListener(OnSave);
        _load.onClick.AddListener(OnLoad);
        _connections.onClick.AddListener(OnConnections);
        _removal.onClick.AddListener(OnRemoval);

        UnityEngine.Windows.Directory.CreateDirectory(Application.streamingAssetsPath);
    }
}
