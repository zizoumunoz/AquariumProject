using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraitSO", menuName = "Scriptable Objects/TraitSO")]
public class TraitSO : SerializedScriptableObject
{
    public string traitName;
    public Color traitColor;

    [Header("Multipliers, 1 = normal")]
    [SerializeField] public Dictionary<StatsEnum, float> multiplier = new();
    public float priceMultiplier = 1f;
}
