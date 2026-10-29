using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketSlotUI : MonoBehaviour
{
    [SerializeField] private Image fishImage;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private Button buyButton;

    public event Action<FishSO> Bought;

    [SerializeField] private FishSO fish;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    public void Set(FishSO so)
    {
        fish = so;
        Debug.Log(transform.position);
        fishImage.sprite = so.sprite;
        nameLabel.text = so.fishName;
        priceLabel.text = "$" + so.price.ToString();
    }

    private void OnBuyClicked()
    {
        Debug.Log(fish);
        if (fish != null && Bought != null) Bought(fish);
    }
}