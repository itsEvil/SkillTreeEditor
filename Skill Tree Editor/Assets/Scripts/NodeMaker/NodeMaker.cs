using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class NodeMaker : MonoBehaviour
{
    [SerializeField] private RectTransform _statsRect, _nodeEffectRect;
    [SerializeField] private Button _complete, _percentage, _remove;
    [SerializeField] private TMP_Dropdown _nodeTypeDropdown, _statTypeDropdown, _nodeEffectDropdown;
    [SerializeField] private TMP_InputField _statAmountInputField, _titleInputField, _descInputField, _idInputField;
    [SerializeField] private TMP_Text _percentageText, _typeText;
    public void OnEnable()
    {
        _nodeTypeDropdown.onValueChanged.AddListener(OnNodeType);
        _complete.onClick.AddListener(OnComplete);
        _percentage.onClick.AddListener(TogglePercentage);
        _remove.onClick.AddListener(OnRemove);
    }
    public void OnDisable()
    {
        _percentage.onClick.RemoveAllListeners();
        _complete.onClick.RemoveAllListeners();
        _remove.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}
