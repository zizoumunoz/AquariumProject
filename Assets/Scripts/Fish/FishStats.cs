using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishStats : SerializedMonoBehaviour
{
    public FishSO baseFish;
    [SerializeField] public Dictionary<StatsEnum, float> statsMultiplier = new();
    public List<TraitSO> traits = new();
    public IFishBehaviour[] behaviours;
    public float priceMultiplier;
    public int Price
    {
        get
        {
            return (int)(baseFish.price * priceMultiplier * (age == Age.Young ? 0.5f : 2.0f));
        }
    }
    public Age age = Age.Young;
    public float happiness;
    public float fullness;
    public FishDiet diet;
    public void Init(FishSO baseFish, List<TraitSO> traits)
    {
        this.baseFish = baseFish;
        this.traits = new(traits);

        for (StatsEnum i = 0; i <= StatsEnum.poopInterval; i++)
        {
            statsMultiplier[i] = 1;
        }

        for (int i = 0; i < traits.Count; i++)
        {
            foreach (var m in traits[i].multiplier)
            {
                if (statsMultiplier.ContainsKey(m.Key))
                {
                    statsMultiplier[m.Key] *= m.Value;
                }
            }
            priceMultiplier *= traits[i].priceMultiplier;
        }

        behaviours = GetComponentsInChildren<IFishBehaviour>();
        behaviours.Sort((x, y) => x.Order - y.Order);


        foreach (IFishBehaviour behaivour in behaviours)
        {
            behaivour.Init(this);
        }
    }
    private void Update()
    {
        foreach (IFishBehaviour behaivour in behaviours)
        {
            behaivour.UpdateFish();
        }

        UpdateHunger();
        UpdateHappiness();
    }
    private void UpdateHunger()
    {
        fullness -= this[StatsEnum.fullnessDecayPerSecond] * Time.deltaTime;
        fullness = Mathf.Clamp(fullness, 0.0f, 100.0f);
    }
    private void UpdateHappiness()
    {
        if (fullness > this[StatsEnum.happyAbove])
        {
            happiness += this[StatsEnum.happinessGainPerSecond] * Time.deltaTime;
        }
        else if (fullness < this[StatsEnum.unhappyBelow])
        {
            happiness -= this[StatsEnum.happinessLossPerSecond] * Time.deltaTime;
        }
        happiness = Mathf.Clamp(happiness, 0.0f, 100.0f);
    }
    public float this[StatsEnum stats]
    {
        get
        {
            return baseFish.baseStats[stats] * statsMultiplier[stats];
        }
    }
    public T GetBehaviour<T>() where T : IFishBehaviour
    {
        return (T)behaviours.First(x => x.GetType() == typeof(T));
    }
}
public enum StatsEnum
{
    baseSize,
    youngSizeMultiplier,

    fishSpeed,
    turnSpeed,
    minRetargetTime,
    maxRetargetTime,

    fullnessDecayPerSecond,
    wanderThreshold,

    unhappyBelow,
    happyAbove,
    happinessLossPerSecond,
    happinessGainPerSecond,

    fleeSpeedMultiplier,

    breedCooldown,
    ageTimer,
    poopInterval,
}