using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _addText,_editText, _titleText;
    private Vector2 _pos;
    private NodeData _data;
    private bool _canEdit;
    public void Init(Vector2 position, NodeData data, bool hasData = false)
    {
        _pos = position;
        _data = data;
        _icon.color = Color.white;
        _canEdit = hasData;
        _titleText.text = data.Title;

        if (_canEdit)
            PopulateUI();
    }

    private void PopulateUI()
    {
        _titleText.text = _data.Title;
        _icon.color = Color.gray;
        _addText.gameObject.SetActive(false);
        _editText.gameObject.SetActive(true);
        _titleText.gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }
    public void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    public void UpdateData(NodeData data)
    {
        _data = data;
        _titleText.text = data.Title;
        //Change picture, etc etc here
    }
    private void OnClick()
    {
        if (!_canEdit) 
        {
            _icon.color = Color.gray;
            _addText.gameObject.SetActive(false);
            _editText.gameObject.SetActive(true);
            _titleText.gameObject.SetActive(true); 
            //_addText.text = "Edit";
            _canEdit = true;
            MainManager.Instance.OnButtonPress(_pos);
            return;
        }

        MainManager.Instance.EditButton(_pos, _data);
        Debug.Log($"We click {_pos} UIButton!");
    }
    public bool CanExport() => _canEdit == true;
    public XElement Export()
    {
        XElement data = new XElement("Skill");
        data.Add(new XElement("X", _pos.x));
        data.Add(new XElement("Y", _pos.y));
        data = _data.Export(data);
        return data;
    }
}
