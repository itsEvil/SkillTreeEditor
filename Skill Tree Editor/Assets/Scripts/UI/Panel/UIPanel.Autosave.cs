using System.Collections;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public partial class UIPanel : MonoBehaviour
{
    private HashSet<string> _autoSavePaths = new HashSet<string>();
    public async void OnAutosave()
    {
        var fileName = $"output-autosave-{Utils.GetCurrentTime()}.xml";
        var directory = Application.streamingAssetsPath;
        var path = Path.Combine(directory, fileName);

        HandleAutosave(path);
        //onSave
        var data = MainManager.Instance.OnSave();

        await File.WriteAllTextAsync(path, data.ToString());
    }
    private void HandleAutosave(string path) //Removes old auto saves
    {
        if (_autoSavePaths.Count < 5)
            _autoSavePaths.Add(path);

        if (_autoSavePaths.Count >= 5)
        {
            var oldPath = _autoSavePaths.First();

            if (!File.Exists(oldPath))
            {
                Debug.LogError($"{oldPath} doesn't exist even though it should?");
                return;
            }

            File.Delete(oldPath);
        }
    }
}

