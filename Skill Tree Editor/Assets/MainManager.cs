using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MainManager : MonoBehaviour
{
    public static int SPACING = 200;
    public static MainManager Instance { get; private set; }
    [SerializeField] private UIButton _buttonPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private UILine _linePrefab;
    //Stores all the button locations
    Dictionary<Vector2, UIButton> _buttons = new Dictionary<Vector2, UIButton>();
    Dictionary<int, UIButton> _id2Button = new Dictionary<int, UIButton>();

    Dictionary<Vector2, UILine> _lines = new Dictionary<Vector2, UILine>();
    
    private int _nextId = 1;
    //HashSet<Vector2> _createdPositions = new HashSet<Vector2>();
    public bool ConnectionMode;
    private Vector2 _lowest, _highest;


    private bool _firstSelected = false;
    private Vector2 _first, _second = Vector2.zero;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        _lowest = _highest = Vector2.zero;
        CreateNewButton(Vector2.zero);
    }
    public void UpdateButton(Vector2 pos, NodeData data)
    {
        if(!_buttons.TryGetValue(pos, out var button))
        {
            Debug.LogError($"We couldnt find button at {pos}!");
            return;
        }

        _id2Button[data.Type] = button;

        button.UpdateData(data);
    }
    public void EditButton(Vector2 pos, NodeData data)
    {
        NodeMaker.Instance.Init(pos, data);
    }
    public void OnButtonPress(Vector2 pos)
    {
        if(!ConnectionMode)
        {
            AddNeighbours(pos);
            return;
        }

        if (!_buttons.TryGetValue(pos, out var button))
        {
            Debug.LogError($"{pos} not found in _buttons");
            return;
        }

        //If we are in connect mode!
        if(!_firstSelected)
        {
            button.ChangeColor(Color.blue);//first here

            _firstSelected = true;
            _first = pos;


            return;
        
        }

        if (!_buttons.TryGetValue(_first, out var first))
        {
            Debug.LogError($"{_first} not found in _buttons");
            return;
        }

        first.ChangeColor(Color.gray);

        if (_first == pos)//we clicked on the same button
            return;
        
        _firstSelected = false;
        _second = pos;

        CreateNewLine(first, button);//button = second here
    }

    private void CreateNewLine(UIButton first, UIButton second)
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
    public void RemoveLines(int id)
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
    public void RemoveLines(Vector2 pos)
    {
        if(!_buttons.TryGetValue(pos, out var button))
        {
            Debug.LogError($"{pos} not found in _buttons");
            return;
        }

        foreach(var connection in button.GetConnections())
        {
            if(!_id2Button.TryGetValue(connection, out var connectedButton))
            {
                Debug.LogError($"{connection} id not found in _id2Button");
                continue;
            }

            var linePos = Vector3.Lerp(pos, connectedButton.transform.localPosition, 0.5f);

            if (!_lines.TryGetValue(linePos, out var line))
            {
                Debug.LogError($"{linePos} not found in _lines");
                continue;
            }

            Destroy(line);
        }
    }

    private void AddNeighbours(Vector2 pos)
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

    private void CreateNewButton(Vector2 position, NodeData data, ConnectionData connections)
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
    private void CreateNewButton(Vector2 position)
    {
        var obj = Instantiate(_buttonPrefab, _container);
        obj.transform.localPosition = position;
        obj.Init(position, NodeData.Empty, ConnectionData.Empty);

        _buttons[position] = obj;
        _container.sizeDelta = new Vector2(
            Vector2.Distance(new Vector2(_lowest.x, 0), new Vector2(_highest.x, 0)) * 2, 
            Vector2.Distance(new Vector2(0, _lowest.y), new Vector2(0, _highest.y)) * 2
            );
    }
    public int GetId()
    {
        while(_id2Button.ContainsKey(_nextId))
        {
            _nextId++;
        }

        return _nextId;
    }
    public NodeData GetNewData(UIButton button)
    {
        int id = GetId();

        _id2Button[id] = button;

        return new NodeData(id, $"Skill Node {id}", string.Empty, string.Empty, NodeReward.None, 0, 0, false);
    }

    public void Remove(Vector2 position)
    {
        if(!_buttons.TryGetValue(position, out var button))
        {
            Debug.LogError("Trying to remove a button that doesn't exist in dict!?");
            return;
        }

        RemoveLines(button.GetId());

        _id2Button.Remove(button.GetId());

        Destroy(button.gameObject);
        _buttons.Remove(position);

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
    }
    public void OnLoad(string text)
    {
        if (!text.Contains("Skills"))
        {
            Debug.Log("Does not contain root tag!");
            return;
        }

        //Clean up
        RemoveAll();

        Debug.Log($"Loaded data: {text}");

        XElement data = XElement.Parse(text);

        foreach(var skillXml in data.Elements("Skill"))
        {
            NodeData nodeData = new(skillXml);
            ConnectionData connectionData = new(skillXml);
            Vector2 position = new(skillXml.ParseFloat("X"), skillXml.ParseFloat("Y"));

            CreateNewButton(position, nodeData, connectionData);

            AddNeighbours(position);

            _nextId = Math.Max(_nextId, nodeData.Type + 1);//Add one because its only our current id that gets read from xml
        }

        foreach(var (id, button) in _id2Button)
        {
            foreach(var connectionId in button.GetConnections().ToArray())
            {
                if(!_id2Button.TryGetValue(connectionId, out var connectedButton))
                {
                    Debug.LogError($"Couldn't find connected button [{id} - {connectionId}]");
                    continue;
                }

                CreateNewLine(button, connectedButton);
            }
        }
    }

    private void RemoveAll()
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
    public XElement OnSave()
    {
        XElement data = new XElement("Skills");
        foreach(var button in _buttons.Values)
        {
            if (!button.CanExport())
                continue;

            data.Add(button.Export());
        }
        return data;
    }
}
