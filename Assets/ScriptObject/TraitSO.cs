using UnityEngine;

[CreateAssetMenu(fileName = "TraitSO", menuName = "Scriptable Objects/TraitSO")]
public class TraitSO : ScriptableObject
{
    [SerializeField]
    public string traitName;
    public Color traitColor;

    [SerializeField]
    [Header("Multipliers, 1 = normal")]
    public FishStatus statusMultiplier = new(1);
    public float priceMultiplier = 1f;
}
