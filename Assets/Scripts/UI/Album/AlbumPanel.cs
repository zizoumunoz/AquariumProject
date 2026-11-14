using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumPanel : MonoBehaviour
{
    [SerializeField] private RawImage _fishImage;
    [SerializeField] private TMP_Text _fishName;

    [SerializeField] private FishSO _fish;

    [SerializeField] private Sprite _undiscoveredSprite;

    //private void Start()
    //{
    //    SetUndiscovered();
    //}

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
