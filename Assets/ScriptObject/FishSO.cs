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
    [SerializeField]
    [Header("Info")]
    public string fishName;
    public Sprite sprite;
    public FishDiet diet = FishDiet.Herbivore;

    [SerializeField]
    [Header("Body")]
    public float baseSize = 1f;
    public float nutrition = 25f;
    public float youngSizeMultiplier = 0.5f;

    [SerializeField]
    [Header("Movement")]
    public float fishSpeed = 1.5f;
    public float minRetargetTime = 2f;
    public float maxRetargetTime = 5f;

    [SerializeField]
    [Header("Hunger Bar")]
    public float startFullness = 100f;
    public float fullnessDecayPerSecond = 2f;
    public float wanderThreshold = 70f;
    public float eatDistance = 0.2f;

    [SerializeField]
    [Header("Extra")]
    public float happiness = 0;
    public float unhappyBelow = 50f;
    public float happyAbove = 70f;
    public float happinessLossPerSecond = 2f;
    public float happinessGainPerSecond = 2f;
    public Age age = Age.Young;

}
