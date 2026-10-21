using UnityEngine;

public class TankManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public static TankManager instance;
    public TankDatabase database;
    [SerializeField] private GameObject tankUIPrefab;
    [SerializeField] private GameObject tankPanel;
    
    private void Start()
    {
        instance = this;
        LoadNewTank(database.tanks[0]);
    }
    public void LoadNewTank(Tank tank)
    {
        //TODO save fish and decoration to the tank database
        foreach (Transform fish in sr.transform)
        {
            Destroy(fish.gameObject);
        }
    }
}
