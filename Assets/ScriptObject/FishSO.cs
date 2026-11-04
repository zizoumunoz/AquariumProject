using UnityEngine;
using System;

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


[CreateAssetMenu(fileName = "FishSO", menuName = "Scriptable Objects/FishSO")]
public class FishSO : ScriptableObject
{
    [Header("Info")]
    public string fishName;
    public Sprite sprite;
    public FishDiet diet = FishDiet.Herbivore;
    public int price = 50;

    [Header("Icon")]
    public SpriteRenderer iconRenderer;
    public StatusIconSO statusIcons;
    public float iconPadding = 0.15f;
    public float iconSize = 1f;


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

    public FishStatus(Func<float, float, float> initialValue, FishStatus parent1, FishStatus parent2)
    {
        baseSize = initialValue(parent1.baseSize, parent2.baseSize);
        nutrition = initialValue(parent1.nutrition, parent2.nutrition);
        youngSizeMultiplier = initialValue(parent1.youngSizeMultiplier, parent2.youngSizeMultiplier);

        fishSpeed = initialValue(parent1.fishSpeed, parent2.fishSpeed);
        minRetargetTime = initialValue(parent1.minRetargetTime, parent2.minRetargetTime);
        maxRetargetTime = initialValue(parent1.maxRetargetTime, parent2.maxRetargetTime);

        startFullness = initialValue(parent1.startFullness, parent2.startFullness);
        fullnessDecayPerSecond = initialValue(parent1.fullnessDecayPerSecond, parent2.fullnessDecayPerSecond);
        wanderThreshold = initialValue(parent1.wanderThreshold, parent2.wanderThreshold);
        eatDistance = initialValue(parent1.eatDistance, parent2.eatDistance);

        happiness = initialValue(parent1.happiness, parent2.happiness);
        unhappyBelow = initialValue(parent1.unhappyBelow, parent2.unhappyBelow);
        happyAbove = initialValue(parent1.happyAbove, parent2.happyAbove);
        happinessLossPerSecond = initialValue(parent1.happinessLossPerSecond, parent2.happinessLossPerSecond);
        happinessGainPerSecond = initialValue(parent1.happinessGainPerSecond, parent2.happinessGainPerSecond);

        fleeSpeedRatio = initialValue(parent1.fleeSpeedRatio, parent2.fleeSpeedRatio);
        fleeDistance = initialValue(parent1.fleeDistance, parent2.fleeDistance);
        fearMemoryTime = initialValue(parent1.fearMemoryTime, parent2.fearMemoryTime);
        detectionRadius = initialValue(parent1.detectionRadius, parent2.detectionRadius);

        breedHappiness = initialValue(parent1.breedHappiness, parent2.breedHappiness);
        breedCooldown = initialValue(parent1.breedCooldown, parent2.breedCooldown);
        breedDistance = initialValue(parent1.breedDistance, parent2.breedDistance);
        ageTime = initialValue(parent1.ageTime, parent2.ageTime);

        poopInterval = initialValue(parent1.poopInterval, parent2.poopInterval);
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
        unhappyBelow = 50f;
        happyAbove = 70f;
        happinessLossPerSecond = 2f;
        happinessGainPerSecond = 2f;

        fleeSpeedRatio = 0.9f;
        fleeDistance = 5f;
        fearMemoryTime = 0.5f;
        detectionRadius = 3f;

        breedHappiness = 90f;
        breedCooldown = 30f;
        breedDistance = 0.5f;
        ageTime = 60f;

        poopInterval = 20f;
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
    public float unhappyBelow = 50f;
    public float happyAbove = 70f;
    public float happinessLossPerSecond = 2f;
    public float happinessGainPerSecond = 2f;

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