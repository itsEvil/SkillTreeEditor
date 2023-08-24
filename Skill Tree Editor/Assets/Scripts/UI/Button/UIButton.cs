using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UIButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _addText, _editText, _titleText;
    public void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }
    public void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
