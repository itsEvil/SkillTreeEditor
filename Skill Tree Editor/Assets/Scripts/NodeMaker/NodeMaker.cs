using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class NodeMaker : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private Button _complete, _remove, _isStart, _addReward;
    [SerializeField] private TMP_Dropdown _classDropdown;
    [SerializeField] private TMP_InputField _titleInputField, _descInputField, _idInputField;
    [SerializeField] private TMP_Text _typeText, _isStartText;

    [SerializeField] private RewardItem _rewardPrefab;

    private List<RewardItem> _rewardItems = new();

    public void OnEnable()
    {
        _addReward.onClick.AddListener(AddNewReward);
        _complete.onClick.AddListener(OnComplete);
        _isStart.onClick.AddListener(OnStart);
        _remove.onClick.AddListener(OnRemove);
    }

    public void OnDisable()
    {
        _isStart.onClick.RemoveAllListeners();
        _addReward.onClick.RemoveAllListeners();
        _complete.onClick.RemoveAllListeners();
        _remove.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}
