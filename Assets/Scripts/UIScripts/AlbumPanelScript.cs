using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumPanelScript : MonoBehaviour
{
    [SerializeField] private Image _fishImage;
    [SerializeField] private TMP_Text _fishName;

    private FishSO _fish;

    public void SetFish(FishSO fishData)
    {
        _fish = fishData;

        _fishImage.sprite = _fish.sprite;
        _fishName.text = _fish.fishName;
    }

    public void SetUndiscovered()
    {
        _fish = null;
        _fishImage.sprite = null;
        _fishName.text = "Not Discovered";

    }
}
