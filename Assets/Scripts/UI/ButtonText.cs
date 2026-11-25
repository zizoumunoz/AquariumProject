using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;
/// <summary>
/// Script to move text along with button
/// </summary>
public class ButtonText : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _pressOffset = 4f;

    private Vector2 _startPosition;
    private bool _isHeld;

    
    private void Awake()
    {
        _startPosition = _text.rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !_button.interactable)
        {
            return;
        }
        _isHeld = true;
        SetPressed();
    }



}
