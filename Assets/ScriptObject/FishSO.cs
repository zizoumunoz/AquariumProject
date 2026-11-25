using UnityEngine;
using System;

public enum FishDiet
{
    Herbivore,
    Carnivore,
    Omnivore,
    AlgaeEater
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


[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : ScriptableObject
{
    [Header("Info")]
    public string fishName;
    public Sprite sprite;
    public FishDiet diet = FishDiet.Herbivore;
    public Rarity rarity = Rarity.Common;
    public int price = 50;
    [TextArea] public string description = "Does stuff";
    public FishStatus fishStatus = new();
}

[Serializable]
public class FishStatus
{

    public FishStatus(float initialValue)
    {
        baseSize = initialValue;
        nutrition = initialValue;
        youngSizeMultiplier = initialValue;

        fishSpeed = initialValue;
        minRetargetTime = initialValue;
        maxRetargetTime = initialValue;

        startFullness = initialValue;
        fullnessDecayPerSecond = initialValue;
        wanderThreshold = initialValue;
        eatDistance = initialValue;

        happiness = initialValue;
        unhappyBelow = initialValue;
        happyAbove = initialValue;
        happinessLossPerSecond = initialValue;
        happinessGainPerSecond = initialValue;
        maxAlgaeLevel = initialValue;

        fleeSpeedRatio = initialValue;
        fleeDistance = initialValue;
        fearMemoryTime = initialValue;
        detectionRadius = initialValue;

        breedHappiness = initialValue;
        breedCooldown = initialValue;
        breedDistance = initialValue;
        ageTime = initialValue;

        poopInterval = initialValue;
    }

    public FishStatus()
    {
        baseSize = 1f;
        nutrition = 25f;
        youngSizeMultiplier = 0.5f;

        fishSpeed = 1.5f;
        minRetargetTime = 2f;
        maxRetargetTime = 5f;

        startFullness = 100f;
        fullnessDecayPerSecond = 2f;
        wanderThreshold = 70f;
        eatDistance = 0.2f;

        happiness = 0;
        unhappyBelow = 40f;
        happyAbove = 60f;
        happinessLossPerSecond = 2f;
        happinessGainPerSecond = 2f;
        maxAlgaeLevel = 50;

        fleeSpeedRatio = 0.9f;
        fleeDistance = 5f;
        fearMemoryTime = 0.5f;
        detectionRadius = 3f;

        breedHappiness = 90f;
        breedCooldown = 30f;
        breedDistance = 1f;
        ageTime = 60f;

        poopInterval = 20f;
    }

    public static FishStatus operator *(FishStatus status1, FishStatus status2)
    {
        FishStatus ret = new()
        {
            baseSize = status1.baseSize * status2.baseSize,
            nutrition = status1.nutrition * status2.nutrition,
            youngSizeMultiplier = status1.youngSizeMultiplier * status2.youngSizeMultiplier,

            fishSpeed = status1.fishSpeed * status2.fishSpeed,
            minRetargetTime = status1.minRetargetTime * status2.minRetargetTime,
            maxRetargetTime = status1.maxRetargetTime * status2.maxRetargetTime,

            startFullness = status1.startFullness * status2.startFullness,
            fullnessDecayPerSecond = status1.fullnessDecayPerSecond * status2.fullnessDecayPerSecond,
            wanderThreshold = status1.wanderThreshold * status2.wanderThreshold,
            eatDistance = status1.eatDistance * status2.eatDistance,

            happiness = status1.happiness * status2.happiness,
            unhappyBelow = status1.unhappyBelow * status2.unhappyBelow,
            happyAbove = status1.happyAbove * status2.happyAbove,
            happinessLossPerSecond = status1.happinessLossPerSecond * status2.happinessLossPerSecond,
            happinessGainPerSecond = status1.happinessGainPerSecond * status2.happinessGainPerSecond,
            maxAlgaeLevel = status1.maxAlgaeLevel * status2.maxAlgaeLevel,

            fleeSpeedRatio = status1.fleeSpeedRatio * status2.fleeSpeedRatio,
            fleeDistance = status1.fleeDistance * status2.fleeDistance,
            fearMemoryTime = status1.fearMemoryTime * status2.fearMemoryTime,
            detectionRadius = status1.detectionRadius * status2.detectionRadius,

            breedHappiness = status1.breedHappiness * status2.breedHappiness,
            breedCooldown = status1.breedCooldown * status2.breedCooldown,
            breedDistance = status1.breedDistance * status2.breedDistance,
            ageTime = status1.ageTime * status2.ageTime,

            poopInterval = status1.poopInterval * status2.poopInterval,
        };

        return ret;
    }

    [Header("Body")]
    public float baseSize = 1f;
    public float nutrition = 25f;
    public float youngSizeMultiplier = 0.5f;

    [Header("Movement")]
    public float fishSpeed = 1.5f;
    public float minRetargetTime = 2f;
    public float maxRetargetTime = 5f;

    [Header("Hunger Bar")]
    public float startFullness = 100f;
    public float fullnessDecayPerSecond = 2f;
    public float wanderThreshold = 70f;
    public float eatDistance = 0.2f;

    [Header("Extra")]
    public float happiness = 0;
    public float unhappyBelow = 40f;
    public float happyAbove = 60f;
    public float happinessLossPerSecond = 2f;
    public float happinessGainPerSecond = 2f;
    public float maxAlgaeLevel = 50;

    [Header("Flee")]
    public float fleeSpeedRatio = 0.9f;
    public float fleeDistance = 5f;
    public float fearMemoryTime = 0.5f;
    public float detectionRadius = 3f;

    [Header("Breeding Targets")]
    public float breedHappiness = 90f;
    public float breedCooldown = 30f;
    public float breedDistance = 0.5f;
    public float ageTime = 60f;

    [Header("Poop")]
    public float poopInterval = 20f;
}