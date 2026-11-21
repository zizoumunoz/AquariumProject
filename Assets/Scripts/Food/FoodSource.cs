using UnityEngine;
using System.Collections.Generic;

public class FoodSource : MonoBehaviour, IFishBehaviour
{
    public FoodType type;
    public float fullnessRestore;
    public bool deleteWhenEaten;
    public int bitesLeft;
    public float size;
    private List<FishController> fishes = new();

    public int Order => 0;

    public void Init(FishStats stats){}

    public void UpdateFish(){}

    public float Eaten()
    {
        bitesLeft--;
        if (bitesLeft == 0 && deleteWhenEaten)
        {
            Destroy(gameObject);
        }
        return fullnessRestore;
    }
    public void AddTarget(FishController controller)
    {
        fishes.Add(controller);
    }
    public void RemoveTarget(FishController controller)
    {
        fishes.Remove(controller);
    }
    public bool IsTargeted()
    {
        for (int i = 0; i < fishes.Count; i++)
        {
            if (fishes[i] == null)
            {
                fishes.RemoveAt(i);
                i--;
            }
        }
        return fishes.Count > 0;
    }
    public bool IsAvailable()
    {
        return bitesLeft - fishes.Count > 0;
    }
}
