using UnityEngine;

[CreateAssetMenu(fileName = "TraitSO", menuName = "Scriptable Objects/TraitSO")]
public class TraitSO : ScriptableObject
{
    [SerializeField]
    public string traitName;

    [SerializeField]
    [Header("Multipliers, 1 = normal")]
    public float sizeMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float turnSpeedMultiplier = 1f;
    public float hungerDecayMultiplier = 1f;
}
