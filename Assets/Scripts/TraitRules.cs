using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TraitRules
{
    
    [Header("Chances (0 to 1)")]
    [SerializeField]
    [Range(0f, 1f)] public float purchaseChance = 0.2f;
    [Range(0f, 1f)] public float inheritChance = 0.5f;
    [Range(0f, 1f)] public float mutationChance = 0.1f;

    public List<TraitSO> RollPurchase(FishDatabaseSO database)
    {
        List<TraitSO> result = new List<TraitSO>();
        if (Random.value < purchaseChance) AddRandom(result, database.traits);
        return result;
    }

    public List<TraitSO> RollChild(List<TraitSO> parentA, List<TraitSO> parentB, FishDatabaseSO database)
    {
        List<TraitSO> result = new List<TraitSO>();

        TryInherit(parentA, result);
        TryInherit(parentB, result);

        if (Random.value < mutationChance) AddRandom(result, database.traits);
        return result;
    }

    private void TryInherit(List<TraitSO> parent, List<TraitSO> result)
    {
        if (Random.value >= inheritChance) return;
        AddRandom(result, parent);
    }

    private void AddRandom(List<TraitSO> result, List<TraitSO> source)
    {
        List<TraitSO> candidates = new List<TraitSO>();
        foreach (TraitSO trait in source)
        {
            if (trait != null && !result.Contains(trait)) candidates.Add(trait);
        }

        if (candidates.Count == 0) return;
        result.Add(candidates[Random.Range(0, candidates.Count)]);
    }
}