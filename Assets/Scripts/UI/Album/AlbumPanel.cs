using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlbumPanel : MonoBehaviour, IPointerClickHandler
{

    [Header("FishSO")]
    [SerializeField] private RawImage _fishImage;
    [SerializeField] private TMP_Text _fishName;
    [SerializeField] private FishSO _fish;
    [SerializeField] private Sprite _undiscoveredSprite;

    // Event so that when a panel is clicked, it sends a message to the LorePanelScreen
    // to display the fish's info.
    public event System.Action<FishSO> Clicked;

    // called when slot is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if (_fish != null)
        {
            Clicked?.Invoke(_fish);
        }
    }
    public void SetFish(FishSO fishData)
    {
        _fish = fishData;

        _fishImage.texture = _fish.sprite.texture;
        _fishName.text = _fish.fishName;
    }

    public void SetUndiscovered()
    {
        _fish = null;
        _fishImage.texture = _undiscoveredSprite.texture;
        _fishName.text = "Not Discovered";

    }
}
