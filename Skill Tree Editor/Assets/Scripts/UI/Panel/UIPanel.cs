using System.Collections;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;

public partial class UIPanel : MonoBehaviour
{
    public static UIPanel Instance { get ; private set; }   

    private bool _connectionsMode = false;
    private bool _removalMode = false;
    private string _path;
    private void OnConnections()
    {
        if(_removalMode)
        {
            Debug.LogWarning("Cannot go into connections mode when removal mode is on!");
            return;
        }

        _connectionsMode = !MainManager.ConnectionMode;

        _connectionsImage.color = _connectionsMode ? Color.red : Color.white;

        _connectionsText.text = $"Connection Mode: {(_connectionsMode ? "True" : "False")}";

        MainManager.ConnectionMode = _connectionsMode;
    }
    private void OnRemoval()
    {
        if(_connectionsMode)
        {
            Debug.LogWarning("Cannot go into removal mode when connection mode is on!");
            return;
        }

        _removalMode = !MainManager.RemovalMode;

        _removalImage.color = _removalMode ? Color.red : Color.white;

        _removalText.text = $"Removal Mode: {(_removalMode ? "True" : "False")}";

        MainManager.RemovalMode = _removalMode;
    }
    private void OnLoad()
    {
        _path = EditorUtility.OpenFilePanel("Skill Tree File (.xml)", "", "xml");
        StartCoroutine(ReadFile());
    }
    IEnumerator ReadFile()
    {
        UnityWebRequest www = UnityWebRequest.Get(_path);

        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }

        MainManager.Instance.OnLoad(www.downloadHandler.text);
    }
    private async void OnSave()
    {
        //onSave
        var data = MainManager.Instance.OnSave();
        var directory = Application.streamingAssetsPath;
        var fileName = $"output-{Utils.GetCurrentTime()}.xml";
        var path = Path.Combine(directory, fileName);

        //Debug.Log(fileName);

        await System.IO.File.WriteAllTextAsync(path, data.ToString());
    }

    public void OnClose()
    {
        OnSave();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}

