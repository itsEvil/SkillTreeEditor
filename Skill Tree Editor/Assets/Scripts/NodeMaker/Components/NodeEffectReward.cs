using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeEffectReward : MonoBehaviour, IReward
{
    [SerializeField] private TMP_Dropdown _nodeEffectDropdown;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _percText;
    [SerializeField] private Button _isPercentage;
    private bool _isPerc = false;
    public void Init(RewardData data)
    {
        _nodeEffectDropdown.value = data.RewardIndex;
        _isPerc = data.IsPercentage;
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }

    public RewardData Export()
    {
        if (!int.TryParse(_inputField.text, out var rewardAmount)) {
            rewardAmount = 0;
        }

        int rewardIndex = _nodeEffectDropdown.value;
        return new RewardData(NodeReward.NODE_EFFECT, rewardIndex, rewardAmount, _isPerc);
    }
    public void OnEnable()
    {
        _isPercentage.onClick.AddListener(OnPerc);
    }

    private void OnPerc()
    {
        _isPerc = !_isPerc;
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }

    public void OnDisable()
    {
        _isPercentage.onClick.RemoveAllListeners();
    }
}