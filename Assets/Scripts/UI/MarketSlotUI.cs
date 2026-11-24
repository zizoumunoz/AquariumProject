using NUnit.Framework;
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
    [SerializeField] private FishSO fish;
    [SerializeField] private Transform starHolder;
    private int shopIndex = 0;
    [HideInInspector] public int timesBought = 0;

    private void Awake()
    {
        MoneyManager.Instance.moneyChanged += UpdateUI;
    }

    private void Start()
    {
        UpdateUI(MoneyManager.Instance.money);
    }
    private void OnDestroy()
    {
        MoneyManager.Instance.moneyChanged -= UpdateUI;
    }

    public void Set(FishSO so, int shopIndex, int timesBought = 0)
    {
        fish = so;
        this.shopIndex = shopIndex;
        this.timesBought = timesBought;
        fishImage.sprite = so.sprite;
        fishImage.preserveAspect = true;
        nameLabel.text = so.fishName;
        priceLabel.text = "$" + so.price;
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
        if (MarketManager.instance.TryBuy(fish))
        {
            timesBought++;
        }
        TankManager.instance.database.activeTank.timesBought[shopIndex] = timesBought;
        MoneyManager.Instance.UpdateUI();
    }

    public void UpdateUI(int money)
    {
        if (money >= fish.price && timesBought < 2)
        {
            buyButton.interactable = true;
            priceLabel.color = Color.black;
        }
        else
        {
            buyButton.interactable = false;
            priceLabel.color = Color.red;
        }
    }
}