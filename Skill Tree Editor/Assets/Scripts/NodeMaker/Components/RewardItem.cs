using System;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IReward
{
    public void Init(RewardData data) { }
    public abstract RewardData Export();
}
public class RewardItem : MonoBehaviour, IReward
{
    [SerializeField] private TMP_Dropdown _rewardTypeDropdrown;
    [SerializeField] private NodeEffectReward _neReward;
    [SerializeField] private AbilityReward _aReward;
    [SerializeField] private DamageReward _dReward;
    [SerializeField] private StatReward _sReward;
    [SerializeField] private RoFReward _rofReward;
    [SerializeField] private ResourceCostReward _rReward;

    private NodeReward rewardType;
    public void Init(RewardData data)
    {
        rewardType = data.Reward;
        _rewardTypeDropdrown.value = (int)data.Reward;
        HandleRewardType();
    }

    private void HandleRewardType()
    {
        _neReward.gameObject.SetActive(rewardType == NodeReward.NODE_EFFECT);
        _sReward.gameObject.SetActive(rewardType == NodeReward.STAT);
        _aReward.gameObject.SetActive(rewardType == NodeReward.ABILITY_CAST);
        _dReward.gameObject.SetActive(rewardType == NodeReward.DAMAGE);
        _rofReward.gameObject.SetActive(rewardType == NodeReward.RATE_OF_FIRE);
        _rReward.gameObject.SetActive(rewardType == NodeReward.RESOURCE_COST);
    }

    public RewardData Export()
    {
        if (rewardType == NodeReward.NODE_EFFECT)
            return _neReward.Export();
        if(rewardType == NodeReward.STAT) 
            return _sReward.Export();
        if(rewardType == NodeReward.DAMAGE)
            return _dReward.Export();
        if (rewardType == NodeReward.RATE_OF_FIRE)
            return _rofReward.Export();
        if (rewardType == NodeReward.RESOURCE_COST)
            return _rReward.Export();
        if (rewardType == NodeReward.ABILITY_CAST)
            return _aReward.Export();

        return RewardData.Empty;
        //NodeReward type = (NodeReward)_nodeTypeDropdown.value;
        //int rewardIndex = 0;
        //int rewardAmount = 0;
        //
        //if (type == NodeReward.STAT)
        //{
        //    rewardIndex = _statDropdown.value;
        //
        //    if (!int.TryParse(_statAmountInput.text, out rewardAmount))
        //    {
        //        rewardAmount = 0;
        //    }
        //}
        //else if (type == NodeReward.NODE_EFFECT)
        //    rewardIndex = _nodeEffectDropdown.value;
        //
        //return new RewardData(type, rewardIndex, rewardAmount, _percentage);
    }

    public void OnEnable()
    {
        _rewardTypeDropdrown.onValueChanged.AddListener(OnRewardType);
    }
    public void OnDisable()
    {
        _rewardTypeDropdrown.onValueChanged.RemoveAllListeners();
    }
    private void OnRewardType(int arg0)
    {
        rewardType = (NodeReward)arg0;
        HandleRewardType();
    }
}