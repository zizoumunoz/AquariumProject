using UnityEngine;

public class TankManager : MonoBehaviour
{
    [SerializeField] private Sprite[] tankSprites;
    public SpriteRenderer sr;
    public Transform fishRoot;
    public Transform decoRoot;
    public Transform foodRoot;
    public static TankManager instance;
    public TankDatabase database;
    [SerializeField] private UITankController tankUIPrefab;
    [SerializeField] private Transform tankPanel;
    [SerializeField] private FishController fishPrefab;

    private void Start()
    {
        instance = this;
        database.tanks.Clear();
        database.tanks.Add(new Tank());
        LoadNewTank(database.tanks[0]);
    }
    public void LoadNewTank(Tank tank)
    {
        sr.sprite = tankSprites[(int)tank.type];
        //TODO save fish and decoration on pc
        foreach (Transform fish in fishRoot)
        {
            Destroy(fish.gameObject);
        }
        foreach (SaveFish saveFish in tank.fishes)
        {
            FishController newFish = Instantiate(fishPrefab, fishRoot);
            newFish.Init(saveFish.baseFish, saveFish.multiplier, saveFish.traits, saveFish.growTimer);
        }
        foreach (Transform deco in decoRoot)
        {
            Destroy(deco.gameObject);
        }
        foreach (Transform food in foodRoot)
        {
            Destroy(food.gameObject);
        }

        database.activeTank = tank;
        ScreenManager.instance.EmptyStack();
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
            SaveFish saveFish = new()
            {
                baseFish = controller.data,
                multiplier = controller.statusMultiplier,
                traits = controller.traits,
                growTimer = controller.growTimer
            };
            database.activeTank.fishes.Add(saveFish);
        }
    }
}
