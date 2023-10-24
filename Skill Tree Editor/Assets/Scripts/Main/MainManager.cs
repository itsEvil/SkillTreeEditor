using System;
using UnityEditor;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [Tooltip("The larger the number the smaller the line will look | default = 1f (no gap between line and button)")]
    public float LineModifier = 1f;

    [Tooltip("Amount of autosaves before we delete old ones")]
    public int AutosaveCount = 5;

    private float _scale = 1f;
    [Tooltip("On mouse wheel how much do we zoom in / out.\nHolding left shift increases this by 4x")]
    [SerializeField]
    private float _scalePerInput = 0.05f;

    [SerializeField] private UIButton _buttonPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UILine _linePrefab;
    //Stores all the button locations
    private void Awake()
    {
        _timeUntilSave = _saveCooldown;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;

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
        HandleAutoSave();
    }

    private void HandleAutoSave()
    {
        _timeUntilSave -= Time.deltaTime;
    
        if(_timeUntilSave <= 0 && _buttons.Count > 1)//not just starting button
        {
            _timeUntilSave = _saveCooldown;

            UIPanel.Instance.OnAutosave();
        }
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
