using UnityEngine;

public partial class NodeMaker : MonoBehaviour
{
    public static NodeMaker Instance { get; private set; }

    private bool _isStartNode = false;

    private Vector2 _location = Vector2.zero;
    private NodeData _data;
    public void Init(Vector2 pos, NodeData data)
    {
        _data = data;
        PopulateUI(data);
        gameObject.SetActive(true);
        _location = pos;
    }
    private void AddNewReward()
    {
        AddReward(RewardData.Empty);
    }
    private void AddReward(RewardData data)
    {
        var obj = Instantiate(_rewardPrefab, _container);
        obj.Init(data);

        _rewardItems.Add(obj);
    }
    private void PopulateUI(NodeData data)
    {
        _typeText.text = "Type: " + _data.Type;
        _idInputField.text = data.Id;

        if (data.StartClass != -1)
        {
            _classDropdown.value = data.StartClass;
            _isStartText.text = "Is Start: Yes";
        }
        else
        {
            _classDropdown.gameObject.SetActive(false);
            _isStartText.text = "Is Start: No";
        }
        _classDropdown.value = data.StartClass;
        _titleInputField.text = data.Title;
        _descInputField.text = data.Description;

        if(_rewardItems.Count != 0)
        {
            ClearRewards();
        }

        foreach (var reward in data.Rewards)
        {
            AddReward(reward);
        }
    }

    private void ClearRewards()
    {
        for (int i = 0; i < _rewardItems.Count; i++)
        {
            Destroy(_rewardItems[i].gameObject);
        }

        _rewardItems = new();
    }

    private void OnStart()
    {
        _isStartNode = !_isStartNode;
        _isStartText.text = $"Is Start: {(_isStartNode ? "Yes" : "No")}";

        _classDropdown.gameObject.SetActive(_isStartNode);
    }
    private void OnComplete()
    {
        int startClass = !_isStartNode ? -1 : _classDropdown.value;
        int id = _data.Type == 0 || _data.Type == -1 ? MainManager.Instance.GetId() : _data.Type;

        RewardData[] rewards = new RewardData[_rewardItems.Count];

        for(int i =0; i < _rewardItems.Count; i++) 
        {
            var rewardData = _rewardItems[i].Export();
            rewards[i] = rewardData;
        }

        _data = new NodeData(id, _idInputField.text, _titleInputField.text, _descInputField.text, rewards, startClass);

        MainManager.Instance.UpdateButton(_location, _data);
        gameObject.SetActive(false);
    }

    private void OnRemove()
    {
        MainManager.Instance.Remove(_location);
        gameObject.SetActive(false);
    }
}
