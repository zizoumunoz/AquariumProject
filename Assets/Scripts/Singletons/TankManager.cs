using UnityEngine;

public class TankManager : MonoBehaviour
{
    [SerializeField] private Sprite[] tankSprites;
    public SpriteRenderer sr;
    public Transform fishRoot;
    public Transform decoRoot;
    public static TankManager instance;
    public TankDatabase database;
    [SerializeField] private GameObject tankUIPrefab;
    [SerializeField] private GameObject tankPanel;
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
            //newFish.Init(saveFish.baseFish, saveFish.multiplier, saveFish.traits);
        }
        foreach (Transform deco in decoRoot)
        {
            Destroy(deco.gameObject);
        }
    }
}
