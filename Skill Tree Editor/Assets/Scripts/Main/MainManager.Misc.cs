using System.Collections.Generic;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    //Spacing between each button 
    //Changing this value will create weird behaviors with old XML files
    public static int SPACING = 200; 
    public static bool ConnectionMode;

    //"Database"
    private static Dictionary<Vector2, UIButton> _buttons   = new Dictionary<Vector2, UIButton>();
    private static Dictionary<int, UIButton>     _id2Button = new Dictionary<int, UIButton>();
    private static Dictionary<Vector2, UILine>   _lines     = new Dictionary<Vector2, UILine>();
    
    //Button Id's
    private int _nextId = 1;

    private Vector2 _lowest, _highest; //used for figuring out the sizeDelta of the _container

    //used for ConnectionMode
    private bool _firstSelected = false;     
    private Vector2 _first = Vector2.zero;

    public int GetId() //Gets the next Id that doesn't exist in our "Database" 
    {
        while(_id2Button.ContainsKey(_nextId))
        {
            _nextId++;
        }
        return _nextId;
    }
}
