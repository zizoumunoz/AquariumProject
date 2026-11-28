using System;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Transform fishRoot;
    public Transform decoRoot;
    public Transform foodRoot;
    public Transform restRoot;
    public static TankManager instance;
    public TankDatabase database;
    public DecorationsDatabaseSO decoDatabase;
    [SerializeField] private UITankController tankUIPrefab;
    [SerializeField] private Transform tankPanel;
    [SerializeField] private FishController fishPrefab;
    private void Awake()
    {
        instance = this;
        database.tanks.Clear();
    }
    private void Start()
    {
        if (SaveAndLoad.save != null)
        {
            database.tanks = SaveAndLoad.save.tankList;
            for (int i = 0; i < database.tanks.Count; i++)
            {
                database.tanks[i].lastRefresh = 0;
            }
            LoadNewTank(database.tanks[^1]);
        }
        else
        {
            CreateTank(database.tankSOs[0]);
        }
        MarketManager.instance.rolled += AssignTankRoll;
    }
    public void LoadNewTank(Tank tank)
    {
        sr.sprite = database.tankSOs[(int)tank.type].sprite;

        SaveTankData();

        foreach (Transform fish in fishRoot)
        {
            Destroy(fish.gameObject);
        }
        foreach (SaveFish saveFish in tank.fishes)
        {
            FishController newFish = Instantiate(fishPrefab, fishRoot);
            List<TraitSO> traits = new();
            for (int i = 0; i < saveFish.traits.Count; i++)
            {
                traits.Add(database.tankSOs[(int)tank.type].traits[saveFish.traits[i]]);
            }
            newFish.Init(database.tankSOs[(int)tank.type].fishes[saveFish.baseFish],
                traits, saveFish.growTimer);
        }
        foreach (Transform deco in decoRoot)
        {
            Destroy(deco.gameObject);
        }
        foreach (SaveDecoration saveDecoration in tank.decorations)
        {
            DecorationSpawner.instance.Spawn(decoDatabase.decorations[saveDecoration.baseDecoration], saveDecoration.pos);
        }
        foreach (Transform food in foodRoot)
        {
            Destroy(food.gameObject);
        }
        foreach (Transform rest in restRoot)
        {
            Destroy(rest.gameObject);
        }

        database.activeTank = tank;
        MarketManager.instance.database = database.tankSOs[(int)tank.type];
        MarketManager.instance.Refresh(tank.marketRoll, tank.lastRefresh, tank.timesBought);
        ScreenManager.instance.EmptyStack();
        MoneyManager.Instance.UpdateUI();
    }
    public void UpdateUI()
    {
        SaveTankData();
        int maxTanksUIIndex = tankPanel.childCount;
        for (int i = 0; i < database.tanks.Count; i++)
        {
            UITankController tankController;
            if (i < maxTanksUIIndex - 1)
            {
                tankController = tankPanel.GetChild(i).GetComponent<UITankController>();
            }
            else
            {
                tankController = Instantiate(tankUIPrefab, tankPanel);
            }
            tankController.UpdateTank(database.tanks[i]);
        }
        Transform shopButton = tankPanel.GetChild(maxTanksUIIndex - 1);
        shopButton.SetAsLastSibling();
    }
    public void SaveTankData()
    {
        database.activeTank.fishes.Clear();
        foreach (Transform fish in fishRoot)
        {
            FishController controller = fish.GetComponent<FishController>();
            List<int> saveTraits = new();
            for (int i = 0; i < controller.traits.Count; i++)
            {
                saveTraits.Add(database.tankSOs[(int)database.activeTank.type].traits.IndexOf(controller.traits[i]));
            }
            SaveFish saveFish = new()
            {
                baseFish = database.tankSOs[(int)database.activeTank.type].fishes.IndexOf(controller.data),
                traits = saveTraits,
                growTimer = controller.GrowTimer,
            };
            database.activeTank.fishes.Add(saveFish);
        }
        database.activeTank.decorations.Clear();
        foreach (Transform deco in decoRoot)
        {
            DecorationSOHolder holder = deco.GetComponent<DecorationSOHolder>();
            SaveDecoration saveDeco = new()
            {
                baseDecoration = decoDatabase.decorations.IndexOf(holder.so),
                pos = deco.position
            };
            database.activeTank.decorations.Add(saveDeco);
        }
    }
    public void CreateTank(TankSO tank)
    {
        database.tanks.Add(new Tank
        {
            currentAttibutes = new(tank.startAttributes),
            type = tank.type,
            maxFish = tank.maxFish,
            marketRoll = MarketManager.instance.PickOffers(tank),
            lastRefresh = Time.time,
            timesBought = { 0, 0, 0, 0, 0, 0 }
        });
        ScreenManager.instance.EmptyStack();
        LoadNewTank(database.tanks[^1]);
    }
    public void AssignTankRoll(List<int> rolledFishes)
    {
        database.activeTank.marketRoll = rolledFishes;
        database.activeTank.lastRefresh = Time.time;
        database.activeTank.timesBought = new();
        for (int i = 0; i < rolledFishes.Count; i++)
        {
            database.activeTank.timesBought.Add(0);
        }
    }
}
