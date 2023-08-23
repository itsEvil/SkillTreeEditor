using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILine : MonoBehaviour
{
    //[SerializeField] private Image _lineIcon;
    public void Init(Vector2[] positions)
    {
        //_from = from;
        //_to = to;

        RectTransform rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.Lerp(positions[0], positions[1], 0.5f);

        rectTransform.rotation = Quaternion.Euler(0f, 0f, (float)Math.Atan2(positions[1].y - positions[0].y, positions[1].x - positions[0].x) * Mathf.Rad2Deg);

        rectTransform.sizeDelta = new Vector2(Vector3.Distance(positions[0], positions[1]) / 2f, rectTransform.sizeDelta.y);
    }
}
