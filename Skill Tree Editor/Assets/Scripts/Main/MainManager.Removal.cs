using System.Collections.Generic;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public void RemoveLines(int id)//Removes lines based on button Id
    {
        if (!_id2Button.TryGetValue(id, out var button))
        {
            Debug.LogError($"{id} not found in _buttons");
            return;
        }

        var pos = button.transform.localPosition;

        foreach (var connection in button.GetConnections())
        {
            if (!_id2Button.TryGetValue(connection, out var connectedButton))
            {
                Debug.LogError($"{connection} id not found in _id2Button, probably already removed!");
                continue;
            }

            var linePos = Vector3.Lerp(pos, connectedButton.transform.localPosition, 0.5f);

            if (!_lines.TryGetValue(linePos, out var line))
            {
                Debug.LogError($"{linePos} not found in _lines");
                continue;
            }

            _lines.Remove(linePos);
            Destroy(line.gameObject);
        }
    }
    public void RemoveLines(Vector2 pos) //removes all lines connected to a button
    {
        if(!_buttons.TryGetValue(pos, out var button))
        {
            Debug.LogError($"{pos} not found in _buttons");
            return;
        }

        foreach(var connection in button.GetConnections()) //checks all connections
        {
            if(!_id2Button.TryGetValue(connection, out var connectedButton))//if it doesn't exists it skips that connection
            {
                Debug.LogError($"{connection} id not found in _id2Button");
                continue;
            }

            var linePos = Vector3.Lerp(pos, connectedButton.transform.localPosition, 0.5f); //figures out line position

            if (!_lines.TryGetValue(linePos, out var line))//checks that position for a line from db
            {
                Debug.LogError($"{linePos} not found in _lines");
                continue;
            }

            Destroy(line); //kaboom goes line
        }
    }
    public void Remove(Vector2 position) //Removes a specific button 
    {
        if(!_buttons.TryGetValue(position, out var button))//checks if this position exists in vector db
        {
            Debug.LogError("Trying to remove a button that doesn't exist in dict!?");
            return;
        }

        RemoveLines(button.GetId()); //removes lines connected to this button

        _id2Button.Remove(button.GetId()); //removes button from id db

        Destroy(button.gameObject); //destorys button
        _buttons.Remove(position); //removes button from vector db

        bool hasValidNeighbour = false;
        foreach (var neighbourOfRemoved in Utils.GetNeighbours(position))//Loop through all the neighbours of the button we remove
        {
            if (!_buttons.TryGetValue(neighbourOfRemoved, out var neighbourOfRemovedButton))//if we cant find a neighbour just skip
                continue;

            if (neighbourOfRemovedButton.CanExport())
            {
                hasValidNeighbour = true;
                continue;
            }

            bool remove = true;

            foreach(var neighbourOfNeighbour in Utils.GetNeighbours(neighbourOfRemoved))//loop through all of the neighbours neighbours
            {
                if (!_buttons.TryGetValue(neighbourOfNeighbour, out var neighbourOfNeighbourButton)) 
                    continue;

                if(neighbourOfNeighbourButton.CanExport()) //if one of the neighbours neighbours buttons can export means
                                                           //that it has data therefore we keep this neighbour alive
                {
                    remove = false;
                    break;
                }
            }

            if (remove)
            {
                Destroy(neighbourOfRemovedButton.gameObject);
                _buttons.Remove(neighbourOfRemoved);
            }
        }

        if(hasValidNeighbour)
            CreateNewButton(position);

        if(_buttons.Count == 0)
        {
            _lowest = _highest = Vector2.zero;
            CreateNewButton(Vector2.zero);
        }
    }
    private void RemoveAll() //Destroys every button and line.
    {
        _container.sizeDelta = _lowest = _highest = Vector2.zero;

        foreach(var button in _buttons.Values)
        {
            Destroy(button.gameObject);
        }

        foreach(var line in _lines.Values)
        {
            Destroy(line.gameObject);
        }

        _lines = new Dictionary<Vector2, UILine>();
        _id2Button = new Dictionary<int, UIButton>();
        _buttons = new Dictionary<Vector2, UIButton>();
    }
}
