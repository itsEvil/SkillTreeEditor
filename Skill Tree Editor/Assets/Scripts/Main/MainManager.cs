using Unity.Collections;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [ReadOnlyAttribute]
    [SerializeField] private string _ = "The larger the number the smaller the line will be";//
    [ReadOnlyAttribute]
    [SerializeField] private string __ = "Default = 1f (no gap between button and line)";//
    public float LineModifier = 1f; //The larger the number the smaller the line will look | default = 1f (no gap meaning its easier to tell where the line points to)
    
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
