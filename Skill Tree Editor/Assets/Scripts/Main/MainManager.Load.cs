using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public void OnLoad(string text) //text param is the entire XML file
    {
        if (text.Length == 0 || !text.Contains("Skills"))//checks if the xml file has a the root tag "Skills"
        {
            Debug.Log("Does not contain root tag!");
            return;
        }

        RemoveAll(); //removes all existing ui

        //Debug.Log($"Loaded data: {text}");

        XElement data = XElement.Parse(text);

        foreach(var skillXml in data.Elements("Skill")) //Loops through all the Skill tags in xml file
        {
            //loads all the data
            NodeData nodeData = new(skillXml);
            ConnectionData connectionData = new(skillXml);
            Vector2 position = new(skillXml.ParseFloat("X"), skillXml.ParseFloat("Y"));

            CreateNewButton(position, nodeData, connectionData); //makes a new button based on the data

            AddNeighbours(position); //adds the Empty neighbour nodes 

            _nextId = Math.Max(_nextId, nodeData.Type + 1);//Add one because its only our current id that gets read from xml
        }

        foreach(var (id, button) in _id2Button)//once all the buttons are loaded we load all the connections (lines) for them
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
}
