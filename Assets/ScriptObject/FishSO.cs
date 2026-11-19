using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : SerializedScriptableObject
{
    [Header("Info")]
    public string fishName;
    public Sprite sprite;
    public FishDiet diet = FishDiet.Herbivore;
    public Rarity rarity = Rarity.Common;
    public int price = 50;

    [Header("Icon")]
    public SpriteRenderer iconRenderer;
    public StatusIconSO statusIcons;
    public float iconPadding = 0.15f;
    public float iconSize = 1f;

    [SerializeField]
    public Dictionary<StatsEnum, float> baseStats = new()
    {
        {StatsEnum.baseSize, 1f },
        {StatsEnum.youngSizeMultiplier, 0.5f },
        
        {StatsEnum.fishSpeed, 1.5f },
        {StatsEnum.minRetargetTime, 2f },
        {StatsEnum.maxRetargetTime, 5f },
        
        {StatsEnum.fullnessDecayPerSecond, 2f },
        {StatsEnum.wanderThreshold, 70f },
        
        {StatsEnum.unhappyBelow, 40f },
        {StatsEnum.happyAbove, 60f },
        {StatsEnum.happinessLossPerSecond, 2f },
        {StatsEnum.happinessGainPerSecond, 2f },
        
        {StatsEnum.fleeSpeedMultiplier, 0.9f },

        {StatsEnum.breedCooldown, 30f },
        {StatsEnum.ageTimer, 60f },

        {StatsEnum.poopInterval, 20f }
    };

    private void OnValidate()
    {
        baseStats = new()
    {
        {StatsEnum.baseSize, 1f },
        {StatsEnum.youngSizeMultiplier, 0.5f },

        {StatsEnum.fishSpeed, 1.5f },
        {StatsEnum.minRetargetTime, 2f },
        {StatsEnum.maxRetargetTime, 5f },

        {StatsEnum.fullnessDecayPerSecond, 2f },
        {StatsEnum.wanderThreshold, 70f },

        {StatsEnum.unhappyBelow, 40f },
        {StatsEnum.happyAbove, 60f },
        {StatsEnum.happinessLossPerSecond, 2f },
        {StatsEnum.happinessGainPerSecond, 2f },

        {StatsEnum.fleeSpeedMultiplier, 0.9f },

        {StatsEnum.breedCooldown, 30f },
        {StatsEnum.ageTimer, 60f },

        {StatsEnum.poopInterval, 20f }
        };
    }
}
public enum FishDiet
{
    Herbivore,
    Carnivore,
    Omnivore
}

public enum Age
{
    Young,
    Adult
}

public enum Rarity
{
    Common,
    Rare,
    Legendary,
}