using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatReward : MonoBehaviour, IReward
{
    [SerializeField] private TMP_Dropdown _statTypeDropdown;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _percText;
    [SerializeField] private Button _isPercentage;
    private bool _isPerc = false;
    public void Init(RewardData data)
    {
        _isPerc = data.IsPercentage;
        _inputField.text = data.RewardAmount.ToString();
        _statTypeDropdown.value = data.RewardIndex;
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }
    public RewardData Export()
    {
        var rewardIndex = _statTypeDropdown.value;

        if (!int.TryParse(_inputField.text, out var rewardAmount)) {
            rewardAmount = 0;
        }
        return new RewardData(NodeReward.STAT, rewardIndex, rewardAmount, _isPerc);
    }
    private void OnPerc()
    {
        _isPerc = !_isPerc;
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }
    public void OnEnable()
    {
        _isPercentage.onClick.AddListener(OnPerc);
    }
    public void OnDisable()
    {
        _isPercentage.onClick.RemoveAllListeners();
    }
}