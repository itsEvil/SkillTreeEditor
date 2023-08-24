using System.Collections.Generic;
using UnityEngine;
public partial class UIButton
{
    private HashSet<int> _connections = new HashSet<int>();

    private Vector2 _pos;
    private NodeData _data;
    private ConnectionData _connectionData;
    private bool _canEdit;
    public void Init(Vector2 position, NodeData data, ConnectionData connections, bool hasData = false)
    {
        _pos = position;
        _data = data;
        _icon.color = Color.white;
        _canEdit = hasData;
        _titleText.text = data.Title;
        _connectionData = connections;

        foreach(var connection in _connectionData.Connections)
            _connections.Add(connection);

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
}
