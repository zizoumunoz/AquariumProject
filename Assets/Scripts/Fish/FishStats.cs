using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class FishStats : SerializedMonoBehaviour
{
    public FishSO baseFish;
    [SerializeField] public Dictionary<StatsEnum, float> statsMultiplier = new();
    public List<TraitSO> traits = new();
    public IFishBehaviour[] behaviours;
    public int price;
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
        }

        behaviours = GetComponents<IFishBehaviour>();

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
    }
    public float this[StatsEnum stats]
    {
        get
        {
            return baseFish.baseStats[stats] * statsMultiplier[stats];
        }
    }
}
public enum StatsEnum
{
    baseSize,
    youngSizeMultiplier,

    fishSpeed,
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