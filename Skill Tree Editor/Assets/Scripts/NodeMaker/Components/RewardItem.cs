using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private Button _isPercentage;
    [SerializeField] private TMP_Dropdown _nodeTypeDropdown, _nodeEffectDropdown, _statDropdown;
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField] private TMP_InputField _statAmountInput;
    [SerializeField] private GameObject _stats, _nodeEffect;

    private bool _percentage = false;
    public void Init(RewardData data)
    {
        _percentage = data.IsPercentage;
        _nodeTypeDropdown.value = (int)data.Reward;
        _statDropdown.value = data.RewardIndex;
        _statAmountInput.text = data.RewardAmount.ToString();
        UpdatePercentageText();

        OnNodeType((int)data.Reward);
    }
    public RewardData Export()
    {
        NodeReward type = (NodeReward)_nodeTypeDropdown.value;
        int rewardIndex = 0;
        int rewardAmount = 0;

        if (type == NodeReward.STAT)
        {
            rewardIndex = _statDropdown.value;

            if (!int.TryParse(_statAmountInput.text, out rewardAmount))
            {
                rewardAmount = 0;
            }
        }
        else if (type == NodeReward.NODE_EFFECT)
            rewardIndex = _nodeEffectDropdown.value;

        return new RewardData(type, rewardIndex, rewardAmount, _percentage);
    }
    private void UpdatePercentageText()
    {
        _percentageText.text = $"Is Percentage: {(_percentage ? "Yes" : "No")}";
    }

    public void OnEnable()
    {
        _isPercentage.onClick.AddListener(OnPercentage);
        _nodeTypeDropdown.onValueChanged.AddListener(OnNodeType);
    }

    private void OnNodeType(int value)
    {
        _stats.SetActive(value == 1);
        _nodeEffect.SetActive(value == 2);
    }

    private void OnPercentage()
    {
        _percentage = !_percentage;
        UpdatePercentageText();
    }

    public void OnDisable()
    {
        Dispose();
    }
    public void OnDestroy()
    {
        Dispose();
    }

    private void Dispose()
    {
        _isPercentage.onClick.RemoveAllListeners();
        _nodeTypeDropdown.onValueChanged.RemoveAllListeners();
    }
}