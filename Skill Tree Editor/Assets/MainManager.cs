using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static int SPACING = 100;
    public static MainManager Instance { get; private set; }
    [SerializeField] private UIButton _buttonPrefab;
    [SerializeField] private RectTransform _container;
    //Stores all the button locations
    Dictionary<Vector2, UIButton> _buttons = new Dictionary<Vector2, UIButton>();
    //HashSet<Vector2> _createdPositions = new HashSet<Vector2>();

    private Vector2 _lowest, _highest;

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

        button.UpdateData(data);
    }
    public void EditButton(Vector2 pos, NodeData data)
    {
        NodeMaker.Instance.Init(pos, data);
    }
    public void OnButtonPress(Vector2 pos)
    {
        AddNeighbours(pos);
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

    private void CreateNewButton(Vector2 position, NodeData data)
    {
        var obj = Instantiate(_buttonPrefab, _container);
        obj.transform.localPosition = position;
        obj.Init(position, data, true);

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
        obj.Init(position, NodeData.Empty);

        _buttons[position] = obj;

        _container.sizeDelta = new Vector2(
            Vector2.Distance(new Vector2(_lowest.x, 0), new Vector2(_highest.x, 0)) * 2, 
            Vector2.Distance(new Vector2(0, _lowest.y), new Vector2(0, _highest.y)) * 2
            );
    }
    public void Remove(Vector2 position)
    {
        if(!_buttons.TryGetValue(position, out var button))
        {
            Debug.LogError("Trying to remove a button that doesn't exist in dict!?");
            return;
        }

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
            Vector2 position = new(skillXml.ParseFloat("X"), skillXml.ParseFloat("Y"));

            CreateNewButton(position, nodeData);

            AddNeighbours(position);
        }
    }

    private void RemoveAll()
    {
        _container.sizeDelta = _lowest = _highest = Vector2.zero;

        foreach(var button in _buttons.Values)
        {
            Destroy(button.gameObject);
        }

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
