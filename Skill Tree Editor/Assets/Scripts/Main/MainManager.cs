using System;
using TMPro.EditorUtilities;
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
    
    private float _scale = 1f;
    [SerializeField]
    private float _scalePerInput = 0.05f;

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
    public void Update()
    {
        HandleZoom();
        ResetZoom();
    }

    private void ResetZoom()
    {
        if(Input.GetKeyDown(KeyCode.Mouse2))
        {
            _scale = 1f;
            _container.localScale = Vector3.one;
        }
    }

    private void HandleZoom()
    {
        bool doubleZoom = false;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            doubleZoom = true;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float amount = _scalePerInput * Input.mouseScrollDelta.y;

            if (doubleZoom)
                amount *= 4;

            _scale += amount;

            _scale = Math.Min(2f, _scale);
            _scale = Math.Max(0.2f, _scale);

            _container.localScale = new Vector3(_scale, _scale);
        }
    }
}
