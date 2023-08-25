using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseScript : MonoBehaviour
{
    [SerializeField] private Button _button;
    public void Awake()
    {
        _button.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        UIPanel.Instance.OnClose();
    }
}
