using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    [SerializeField] private FishDatabaseSO database;
    [SerializeField] private float refreshInterval = 60f;
    [SerializeField] private int offerCount = 6;

    [SerializeField] private MarketPanel panel;
    private float lastRefreshTime = float.NegativeInfinity;

    private void Awake()
    {
        instance = this;
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

    public void TryBuy(FishSO fish)
    {
        if (MoneyManager.Instance.money < fish.price) return;
        if (TankManager.instance.fishRoot.childCount == TankManager.instance.database.activeTank.maxFish)
        {
            return;
        }

        MoneyManager.Instance.SpendMoney(fish.price);
        FishSpawner.instance.Spawn(fish);
    }
}