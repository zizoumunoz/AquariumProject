using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishLorePanel : MonoBehaviour
{
    [SerializeField] private Image _fishImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _statsText;

    private void OnEnable()
    {
        Clear();
    }

    public void Show(FishSO fish)
    {
        _fishImage.enabled = true;
        _fishImage.sprite = fish.sprite;
        _fishImage.preserveAspect = true;
        _nameText.text = fish.fishName;
        _descriptionText.text = "Sample description";
        _statsText.text =
            $"Rarity: {fish.rarity}\n" +
            $"Diet: {fish.diet}\n" +
            $"Price: {fish.price}\n" +
            $"Speed: {fish.fishStatus.fishSpeed}\n" +
            $"Size: {fish.fishStatus.baseSize}\n";
    }

    public void Clear()
    {
        _fishImage.enabled = false;
        _nameText.text = "Select a fish";
        _descriptionText.text = "";
        _statsText.text = "";
    }
}
