using UnityEngine;
using UnityEngine.UI;

public class TankManager : MonoBehaviour
{
    [SerializeField] private Sprite[] tankSprites;
    public SpriteRenderer sr;
    public Transform fishRoot;
    public Transform decoRoot;
    public static TankManager instance;
    public TankDatabase database;
    [SerializeField] private UITankController tankUIPrefab;
    [SerializeField] private Transform tankPanel;
    [SerializeField] private FishController fishPrefab;

    private void Start()
    {
        instance = this;
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
            newFish.Init(saveFish.baseFish, saveFish.multiplier, saveFish.traits);
        }
        foreach (Transform deco in decoRoot)
        {
            Destroy(deco.gameObject);
        }
    }
    public void UpdateUI()
    {
        // Save active tank to database
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
            if (database.tanks[i] == database.activeTank)
            {
                tankController.GetComponent<Button>().enabled = false;
            }
        }
        Transform shopButton = tankPanel.GetChild(maxTanksUIIndex - 1);
        shopButton.SetAsLastSibling();
    }
}
