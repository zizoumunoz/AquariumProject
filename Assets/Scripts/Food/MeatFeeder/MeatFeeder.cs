using UnityEngine;

[RequireComponent(typeof(FoodReservoir))]
public class MeatFeeder : MonoBehaviour
{
    [SerializeField] private MeatFeederSO data;
    [SerializeField] private Food foodPrefab;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private Transform foodRoot;

    private FoodReservoir reservoir;
    private float timer;

    private void Awake()
    {
        reservoir = GetComponent<FoodReservoir>();
        reservoir.Configure(data.dropsBeforeReload, data.reloadTime);
    }

    private void Update()
    {
        if (reservoir.IsReloading) return;

        timer += Time.deltaTime;
        if (timer < data.dropInterval) return;

        timer = 0f;
        Dispense();
    }

    public void Dispense()
    {
        if (!reservoir.TryTake()) return;

        Food meat = Instantiate(foodPrefab, foodRoot);
        meat.transform.position = dropPoint.position;
        meat.Init(data.food);
    }
}