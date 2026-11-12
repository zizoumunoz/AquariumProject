using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDatabase", menuName = "Scriptable Objects/FishDatabase")]
public class FishDatabaseSO : ScriptableObject
{
    public List<FishSO> fishes = new();
    public List<TraitSO> traits = new();
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
        List<TraitSO> pool = new(traits);
        List<TraitSO> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }
    private void OnValidate()
    {
        fishes.Sort((x, y) => x.rarity - y.rarity);
    }
}