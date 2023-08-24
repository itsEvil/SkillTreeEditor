using UnityEngine;
public partial class MainManager : MonoBehaviour
{
    private void CreateNewLine(UIButton first, UIButton second)//Creates a line by figuring out where to put it
                                                               //from the location of two buttons
    {
        var position = Vector3.Lerp(first.transform.localPosition, second.transform.localPosition, 0.5f);

        if (_lines.ContainsKey(position))
        {
            Debug.Log($"Line already exists at {position}");
            return;
        }

        var obj = Instantiate(_linePrefab, _container);
            obj.Init(new Vector2[2] { first.transform.localPosition, second.transform.localPosition });

        _lines[position] = obj;

        Debug.Log($"Trying to add {first.GetType()} to {second.GetId()}");

        first.AddConnection(second.GetId());
        second.AddConnection(first.GetId());
    }

    private void AddNeighbours(Vector2 pos)//Logic to add neighbours
    {
        foreach (var position in Utils.GetNeighbours(pos)) //Workout neighbours
        {
            if (!_buttons.ContainsKey(position)) //Does neighbour location exist in our "db"
            {
                _lowest = Vector2.Min(position, _lowest); //if no add a new button for that location
                _highest = Vector2.Max(position, _highest);

                CreateNewButton(position);
            }
        }
    }
    private void CreateNewButton(Vector2 position, NodeData data, ConnectionData connections)//Spawn a new button WITH data
                                                                                             //update the sizeDelta of our container
    {
        var obj = Instantiate(_buttonPrefab, _container);
            obj.transform.localPosition = position;
            obj.Init(position, data, connections, true);

        _id2Button[data.Type] = obj;

        _buttons[position] = obj;

        _container.sizeDelta = new Vector2(
            Vector2.Distance(new Vector2(_lowest.x, 0), new Vector2(_highest.x, 0)) * 2,
            Vector2.Distance(new Vector2(0, _lowest.y), new Vector2(0, _highest.y)) * 2
            );
    }
    private void CreateNewButton(Vector2 position)//Spawn a new button WITHOUT data (Empty Node)
    {                                             //update the sizeDelta of our container
        var obj = Instantiate(_buttonPrefab, _container);
        obj.transform.localPosition = position;
        obj.Init(position, NodeData.Empty, ConnectionData.Empty);

        _buttons[position] = obj;
        _container.sizeDelta = new Vector2(
            Vector2.Distance(new Vector2(_lowest.x, 0), new Vector2(_highest.x, 0)) * 2, 
            Vector2.Distance(new Vector2(0, _lowest.y), new Vector2(0, _highest.y)) * 2
            );
    }
    public NodeData GetNewData(UIButton button)//Gets a new Id for the edit node when it gets converted
    {
        int id = GetId();

        _id2Button[id] = button;

        return new NodeData(id, $"Skill Node {id}", string.Empty, string.Empty, NodeReward.None, 0, 0, false);
    }
}

    