using UnityEngine;

public enum FishDiet
{
    Herbivore,
    Carnivore,
    Omnivore
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

}
