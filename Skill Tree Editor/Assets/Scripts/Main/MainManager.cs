using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; } 
    [SerializeField] private UIButton _buttonPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UILine _linePrefab;
    //Stores all the button locations
    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        _lowest = _highest = Vector2.zero;
        CreateNewButton(Vector2.zero);
    }
}
