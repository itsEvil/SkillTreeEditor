using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public void UpdateButton(Vector2 pos, NodeData data)//called once the user presses save on the Node Maker
    {
        if(!_buttons.TryGetValue(pos, out var button))
        {
            Debug.LogError($"We couldnt find button at {pos}!");
            return;
        }

        _id2Button[data.Type] = button;

        button.UpdateData(data);
    }
    public void OnButtonPress(Vector2 pos) //When we press a button
    {
        if(!ConnectionMode)
        {
            //if we are not in connection mode that means this was a
            //Empty Node which we convered to a edit node 
            //so we gotta add the neighbours now
            AddNeighbours(pos); 
            return; //return so we don't run the connection mode logic
        }

        //Connection mode logic below

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

        CreateNewLine(first, button, true);//button = second here
    }
}
