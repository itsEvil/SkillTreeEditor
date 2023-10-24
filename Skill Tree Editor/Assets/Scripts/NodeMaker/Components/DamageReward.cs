using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageReward : MonoBehaviour, IReward
{
    [SerializeField] private TMP_Dropdown _damageDropdown;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _percText;
    [SerializeField] private Button _isPercentage;
    private bool _isPerc = false;
    public void Init(RewardData data)
    {
        _isPerc = data.IsPercentage;
        _inputField.text = data.RewardAmount.ToString();
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
        _damageDropdown.value = data.RewardIndex;
    }

    public RewardData Export()
    {
        if (!int.TryParse(_inputField.text, out var rewardAmount)) {
            rewardAmount = 0;
        }
        return new RewardData(NodeReward.DAMAGE, _damageDropdown.value, rewardAmount, _isPerc);
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