using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketSlotUI : MonoBehaviour
{
    [SerializeField] private Image fishImage;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private TMP_Text expensivePriceLabel;
    [SerializeField] private Button buyButton;
    [SerializeField] private FishSO fish;
    [SerializeField] private Transform starHolder;

    private void Awake()
    {
        MoneyManager.Instance.moneyChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        MoneyManager.Instance.moneyChanged -= UpdateUI;
    }

    public void Set(FishSO so)
    {
        fish = so;
        fishImage.sprite = so.sprite;
        nameLabel.text = so.fishName;
        priceLabel.text = "$" + so.price;
        expensivePriceLabel.text = "$" + so.price;
        foreach (Transform star in starHolder)
        {
            star.gameObject.SetActive(false);
        }
        for (int i = 0; i <= (int)fish.rarity; i++)
        {
            starHolder.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnBuyClicked()
    {
        MarketManager.instance.TryBuy(fish);
    }

    public void UpdateUI(int money)
    {
        if (money >= fish.price)
        {
            buyButton.interactable = true;
            expensivePriceLabel.gameObject.SetActive(false);
        }
        else
        {
            buyButton.interactable = false;
            expensivePriceLabel.gameObject.SetActive(true);
        }
    }
}