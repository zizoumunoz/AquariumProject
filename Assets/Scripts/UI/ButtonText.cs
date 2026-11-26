using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering.Universal;
/// <summary>
/// Script to move text along with button
/// </summary>
public class ButtonText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        Refresh();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        _isHeld = false;
        Refresh();
    }

 

    // If the button gets hidden midclick, put text back
    private void OnDisable()
    {
        _isHeld = false;
        SetReleased();
    }

    private void Refresh()
    {
        if (_isHeld && _button.interactable)
        {
            SetPressed();
        }
        else
        {
            SetReleased();
        }
    }

    private void SetPressed()
    {
        _text.rectTransform.anchoredPosition = _startPosition + Vector2.down * _pressOffset;
    }

    private void SetReleased()
    {
        _text.rectTransform.anchoredPosition = _startPosition;
    }
}
