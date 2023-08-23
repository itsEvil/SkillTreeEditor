using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine.Networking;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Button _save, _load, _connections;
    [SerializeField] private TMP_Text _connectionsText;
    private bool _connectionsMode = false;
    private string _path;
    public void Awake()
    {
        _save.onClick.AddListener(OnSave);
        _load.onClick.AddListener(OnLoad);
        _connections.onClick.AddListener(OnConnections);

        UnityEngine.Windows.Directory.CreateDirectory(Application.streamingAssetsPath);
    }

    private void OnConnections()
    {
        _connectionsMode = !MainManager.Instance.ConnectionMode;

        _connectionsText.text = $"Connection Mode: {(_connectionsMode ? "True" : "False")}";

        MainManager.Instance.ConnectionMode = _connectionsMode;
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

        if(www.isNetworkError || www.isHttpError)
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
        var fileName = $"output-{GetCurrentTime()}.xml";
        var path = Path.Combine(directory, fileName);

        Debug.Log(fileName);

        //if (!System.IO.File.Exists(path))
        //{
        //    await System.IO.File.WriteAllTextAsync(path, data.ToString());
        //}
        await System.IO.File.WriteAllTextAsync(path, data.ToString());
    }

    private string GetCurrentTime()
    {
        var time = DateTime.Now;

        return time.ToString("dd-MMM-yyy-hh-mm-ss");
    }
}
