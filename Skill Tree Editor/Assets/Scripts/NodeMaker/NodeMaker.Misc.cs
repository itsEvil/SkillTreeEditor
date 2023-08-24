using UnityEngine;

public partial class NodeMaker : MonoBehaviour
{
    public static NodeMaker Instance { get; private set; }

    private bool _isPercentage = false;
    private Vector2 _location = Vector2.zero;
    private NodeData _data;
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

            if (!int.TryParse(_statAmountInputField.text, out rewardAmount))
            {
                rewardAmount = 0;
            }
        }
        else if (_nodeTypeDropdown.value == 2)
            rewardIndex = _nodeEffectDropdown.value;

        int id = _data.Type;
        if (id == 0 || id == -1)
        {
            id = MainManager.Instance.GetId();
        }

        _data = new NodeData(id, _idInputField.text, _titleInputField.text, _descInputField.text, (NodeReward)_nodeTypeDropdown.value, rewardIndex, rewardAmount, _isPercentage);

        MainManager.Instance.UpdateButton(_location, _data);
        gameObject.SetActive(false);
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
}
