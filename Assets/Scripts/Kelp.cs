using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FoodReservoir))]
public class Kelp : MonoBehaviour, IFoodSource
{
    [SerializeField] private KelpSO data;

    private SpriteRenderer sr;
    private FoodReservoir reservoir;

    public FoodType Type { get { return FoodType.Plant; } }
    public bool IsAvailable { get { return !reservoir.IsReloading; } }
    public Vector2 Position { get { return sr.bounds.center; } }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        reservoir = GetComponent<FoodReservoir>();
        reservoir.Configure(data.bitesBeforeReload, data.reloadTime);
    }

    private void OnEnable()
    {
        FoodSources.Register(this);
    }

    private void OnDisable()
    {
        FoodSources.Unregister(this);
    }

    public float Eat()
    {
        return reservoir.TryTake() ? data.fullnessPerBite : 0f;
    }
}