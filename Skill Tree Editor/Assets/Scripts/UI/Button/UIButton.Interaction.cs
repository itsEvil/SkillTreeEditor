using UnityEngine;
public partial class UIButton
{
    public void UpdateData(NodeData data)
    {
        _data = data;
        _titleText.text = data.Title;
        //Change picture, etc etc here
    }
    public void ChangeColor(Color color)
    {
        _icon.color = color;
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

        if(MainManager.ConnectionMode)
        {
            MainManager.Instance.OnButtonPress(_pos);
            return;
        }

        if(_data.Type == 0)
        {
            _data = MainManager.Instance.GetNewData(this); //over write _data with one that contains a unique id
        }
        NodeMaker.Instance.Init(_pos, _data);

        //Debug.Log($"We click {_pos} UIButton!");
    }
}
