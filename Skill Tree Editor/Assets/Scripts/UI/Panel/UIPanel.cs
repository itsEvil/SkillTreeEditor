using System.Collections;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;

public partial class UIPanel : MonoBehaviour
{
    public static UIPanel Instance { get ; private set; }   

    private bool _connectionsMode = false;
    private string _path;
    private void OnConnections()
    {
        _connectionsMode = !MainManager.ConnectionMode;

        _connectionsText.text = $"Connection Mode: {(_connectionsMode ? "True" : "False")}";

        MainManager.ConnectionMode = _connectionsMode;
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

