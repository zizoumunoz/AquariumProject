using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    [SerializeField] private FishDatabaseSO database;
    [SerializeField] private float refreshInterval = 60f;
    [SerializeField] private int offerCount = 6;

    public event Action<FishSO> FishPurchased;

    [SerializeField] private MarketPanel panel;
    private float lastRefreshTime = float.NegativeInfinity;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        panel.FishBought += TryBuy;
        panel.gameObject.SetActive(false);
    }

    public void OpenMarket()
    {
        if (Time.time - lastRefreshTime >= refreshInterval)
            Refresh();
    }

    public void Refresh()
    {
        lastRefreshTime = Time.time;
        panel.SetOffers(PickOffers());
    }

    private List<FishSO> PickOffers()
    {
        List<FishSO> pool = new(database.fishes);
        List<FishSO> result = new();

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
        if (MoneyDisplay.Instance.money < price) return;

        MoneyDisplay.Instance.AddMoney(-price);
        if (FishPurchased != null) FishPurchased(fish);
    }
}