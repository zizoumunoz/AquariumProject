using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    public TankSO database;
    [SerializeField] private float refreshInterval = 60f;
    [SerializeField] private int offerCount = 6;
    [SerializeField] private float rarityIndex = 2.5f;
    public delegate void MarketRolled(List<FishSO> fishes);
    public MarketRolled rolled;
    [SerializeField] private MarketPanel panel;
    private float lastRefreshTime = float.NegativeInfinity;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Time.time - lastRefreshTime >= refreshInterval)
            Refresh();
    }

    public void Refresh()
    {
        lastRefreshTime = Time.time;
        List<FishSO> marketFish = PickOffers();
        rolled?.Invoke(marketFish);
        panel.SetOffers(marketFish);
    }

    public void Refresh(List<FishSO> marketFish, float lastRefreshTime, List<int> timesBought)
    {
        this.lastRefreshTime = lastRefreshTime;
        panel.SetOffers(marketFish, timesBought);
        rolled?.Invoke(marketFish);
    }

    public List<FishSO> PickOffers(TankSO tank = null)
    {
        List<FishSO> result = new();

        tank = tank != null ? tank : database;

        for (int i = 0; i < offerCount; i++)
        {
            int index = Random.Range(0, (int)Mathf.Pow(tank.fishes.Count - 1, rarityIndex));
            index = Mathf.RoundToInt(Mathf.Pow(index, 1f / rarityIndex));
            result.Add(tank.fishes[tank.fishes.Count - index - 1]);
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
        FishCollection.Discover(fish);
        return true;
    }

}