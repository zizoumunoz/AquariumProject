using UnityEngine;

public class FishNeeds : MonoBehaviour
{
    private FishController fish;
    [SerializeField] private float fullness;
    [SerializeField] private float happiness;

    public float Happiness {  get { return happiness; }}
    public float Fullness {  get { return fullness; }}

    public bool IsHungry
    {
        get { return fullness <= fish.Data.fishStatus.wanderThreshold * fish.statusMultiplier.wanderThreshold; }
    }

    private void Awake()
    {
        fish = GetComponent<FishController>();
    }

    public void Setup()
    {
        fullness = fish.Data.fishStatus.startFullness * fish.statusMultiplier.startFullness;
        happiness = fish.Data.fishStatus.happiness * fish.statusMultiplier.happiness;
    }

    public void Feed(float amount)
    {
        fullness = Mathf.Min(100f, fullness + amount);
    }

    private void Update()
    {
        fullness = Mathf.Max(0.0f, fullness - fish.Data.fishStatus.fullnessDecayPerSecond * Time.deltaTime);
        UpdateHappiness();
    }

    private void UpdateHappiness()
    {
        FishStatus data = fish.Data.fishStatus;
        FishStatus mult = fish.statusMultiplier;

        if (fullness < data.unhappyBelow * mult.unhappyBelow)
        {
            happiness -= data.happinessLossPerSecond * mult.happinessLossPerSecond * Time.deltaTime;
        }
        else if (fullness > data.happyAbove * mult.happyAbove)
        {
            happiness += data.happinessGainPerSecond * mult.happinessGainPerSecond * Time.deltaTime;
        }


        happiness = Mathf.Clamp(happiness, 0f, 100f);
    }

}
