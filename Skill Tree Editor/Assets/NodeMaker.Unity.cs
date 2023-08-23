using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class NodeMaker : MonoBehaviour
{
    public static NodeMaker Instance { get; private set; }

    [SerializeField] private RectTransform _statsRect, _nodeEffectRect;
    [SerializeField] private Button _complete, _percentage, _remove;
    [SerializeField] private TMP_Dropdown _nodeTypeDropdown, _statTypeDropdown, _nodeEffectDropdown;
    [SerializeField] private TMP_InputField _statAmountInputField, _titleInputField, _descInputField, _idInputField;
    [SerializeField] private TMP_Text _percentageText, _typeText;

    private bool _isPercentage = false;
    private Vector2 _location = Vector2.zero;
    private NodeData _data;
    private void TogglePercentage()
    {
        _isPercentage = !_isPercentage;
        UpdatePercentageText();
    }
    private void UpdatePercentageText()
    {
        _percentageText.text = $"IsPercentage: {(_isPercentage ? "Yes" : "No")}";
    }
    private void OnComplete()
    {
        int rewardIndex = 0;
        int rewardAmount = 0;

        if (_nodeTypeDropdown.value == 1)
        {
            rewardIndex = _statTypeDropdown.value;
            
            if(!int.TryParse(_statAmountInputField.text, out rewardAmount)) 
            {
                rewardAmount = 0;
            }
        }
        else if (_nodeTypeDropdown.value == 2)
            rewardIndex = _nodeEffectDropdown.value;

        int id = _data.Type;
        if(id == 0 || id == -1) 
        {
            id = MainManager.Instance.GetId();
        }

        _data = new NodeData(id, _idInputField.text, _titleInputField.text, _descInputField.text, (NodeReward)_nodeTypeDropdown.value, rewardIndex, rewardAmount, _isPercentage);

        MainManager.Instance.UpdateButton(_location, _data);
        gameObject.SetActive(false);
    }
    public void OnEnable()
    {
        _nodeTypeDropdown.onValueChanged.AddListener(OnNodeType);
        _complete.onClick.AddListener(OnComplete);
        _percentage.onClick.AddListener(TogglePercentage);
        _remove.onClick.AddListener(OnRemove);
    }

    private void OnRemove()
    {
        MainManager.Instance.Remove(_location);
        gameObject.SetActive(false);
    }

    private void OnNodeType(int value)
    {
        _statsRect.gameObject.SetActive(value == 1);
        _nodeEffectRect.gameObject.SetActive(value == 2);
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
