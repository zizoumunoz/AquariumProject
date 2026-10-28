using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private FishDatabaseSO database;
    [SerializeField] private MarketPanel panelPrefab;
    [SerializeField] private Transform canvasRoot;
    [SerializeField] private float refreshInterval = 60f;
    [SerializeField] private int offerCount = 6;

    public event Action<FishSO> FishPurchased;

    private MarketPanel panel;
    private float lastRefreshTime = float.NegativeInfinity;

    private void Start()
    {
        panel = Instantiate(panelPrefab, canvasRoot, false);
        panel.FishBought += TryBuy;
        panel.gameObject.SetActive(false);
    }

    public void ToggleMarket()
    {
        if (panel.gameObject.activeSelf) CloseMarket();
        else OpenMarket();
    }

    public void OpenMarket()
    {
        panel.gameObject.SetActive(true);

        if (Time.time - lastRefreshTime >= refreshInterval)
            Refresh();
    }

    public void CloseMarket()
    {
        panel.gameObject.SetActive(false);
    }

    private void Refresh()
    {
        lastRefreshTime = Time.time;
        panel.SetOffers(PickOffers());
    }

    private List<FishSO> PickOffers()
    {
        List<FishSO> pool = new List<FishSO>(database.fishes);
        List<FishSO> result = new List<FishSO>();

        for (int i = 0; i < offerCount && pool.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }

    private void TryBuy(FishSO fish)
    {
        int price = (int)fish.price;
        //if (MoneyDisplay.Instance.money < price) return;

        //MoneyDisplay.Instance.AddMoney(-price);
        if (FishPurchased != null) FishPurchased(fish);
    }
}