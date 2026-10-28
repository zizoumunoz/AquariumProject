using UnityEngine;

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

[System.Serializable]
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

    }

    public FishStatus()
    {

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
}