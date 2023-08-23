using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class NodeMaker : MonoBehaviour
{
    public void Init(Vector2 pos, NodeData data)
    {
        _data = data;
        PopulateUI(data);
        gameObject.SetActive(true);
        _location = pos;
    }
    private void PopulateUI(NodeData data)
    {
        _typeText.text = "Type: " + _data.Type;
        _isPercentage = data.IsPercentage;
        _idInputField.text = data.Id;
        _nodeEffectDropdown.value = data.RewardIndex;
        _nodeTypeDropdown.value = (int)data.Reward;
        _statTypeDropdown.value = data.RewardIndex;
        _statAmountInputField.text = data.RewardAmount.ToString();

        _titleInputField.text = data.Title;
        _descInputField.text = data.Description;

        OnNodeType((int)data.Reward);

        UpdatePercentageText();
    }
}
