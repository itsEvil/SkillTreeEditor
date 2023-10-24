using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityReward : MonoBehaviour, IReward
{
    [SerializeField] private TMP_Dropdown _castType, _abilityType;
    [SerializeField] private TMP_Text _percText;
    [SerializeField] private TMP_InputField _valueInput;
    [SerializeField] private Button _percButton;
    private bool _isPerc = false;
    public void Init(RewardData data)
    {
        _isPerc = data.IsPercentage;
        _castType.value = data.RewardIndex;
        _abilityType.value = data.ExtraInt;
        _valueInput.text = data.RewardAmount.ToString();
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }

    public RewardData Export()
    {
        if (!int.TryParse(_valueInput.text, out var rewardAmount)) {
            rewardAmount = 0;
        }

        return new RewardData(NodeReward.ABILITY_CAST, _castType.value, rewardAmount, _isPerc, _abilityType.value);
    }
    public void OnEnable()
    {
        _percButton.onClick.AddListener(OnPerc);
    }

    private void OnPerc()
    {
        _isPerc = !_isPerc;
        _percText.text = $"Percent: {(_isPerc ? "Yes" : "No")}";
    }

    public void OnDisable()
    {
        _percButton.onClick.RemoveAllListeners();
    }
}