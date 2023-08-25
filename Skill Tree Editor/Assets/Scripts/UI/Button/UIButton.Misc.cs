using System.Collections.Generic;
using System.Linq;
public partial class UIButton
{
    public int GetId()
    {
        if (_data.Type == 0)
            _data = MainManager.Instance.GetNewData(this);

        return _data.Type;
    }
    public void AddConnection(int id)
    {
        _connections.Add(id);
        _connectionData = new ConnectionData(_connections.ToArray());
    }
    public bool HasId(int id)
    {
        return _connections.Contains(id);
    }
    public void RemoveConnection(int id)
    {
        _connections.Remove(id);
        _connectionData = new ConnectionData(_connections.ToArray());
    }
    public HashSet<int> GetConnections() => _connections;
    public bool CanExport() => _canEdit == true;
}
