using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    public FishDatabaseSO database;
    [SerializeField] private float refreshInterval = 60f;
    [SerializeField] private int offerCount = 6;
    [SerializeField] private float rarityIndex = 2.5f;


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
        List<FishSO> result = new();

        for (int i = 0; i < offerCount; i++)
        {
            int index = Random.Range(0, (int)Mathf.Pow(database.fishes.Count - 1, rarityIndex));
            index = Mathf.RoundToInt(Mathf.Pow(index, 1f / rarityIndex));
            result.Add(database.fishes[database.fishes.Count - index - 1]);
        }
        return result;
    }

    public bool TryBuy(FishSO fish)
    {
        if (MoneyManager.Instance.money < fish.price) return false;
        if (TankManager.instance.fishRoot.childCount == TankManager.instance.database.activeTank.maxFish)
        {
            return false;
        }

        FishSpawner.instance.Spawn(fish);
        MoneyManager.Instance.SpendMoney(fish.price);
        return true;
    }

}