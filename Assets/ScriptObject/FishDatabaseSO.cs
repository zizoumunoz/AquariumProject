using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDatabase", menuName = "Scriptable Objects/FishDatabase")]
public class FishDatabaseSO : ScriptableObject
{
    public List<FishSO> fishes = new List<FishSO>();
    public List<TraitSO> traits = new List<TraitSO>();
    public StatusIconSO statusIcons;
    public FishSO GetByName(string name)
    {
        return fishes.Find(f => f.fishName == name);
    }

    public FishSO GetRandom()
    {
        if (fishes.Count == 0) return null;
        return fishes[Random.Range(0, fishes.Count)];
    }

    public List<TraitSO> GetRandomTraits(int count)
    {
        List<TraitSO> pool = new List<TraitSO>(traits);
        List<TraitSO> result = new List<TraitSO>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }
}