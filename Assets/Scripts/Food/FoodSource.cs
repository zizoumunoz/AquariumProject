using UnityEngine;
using System.Collections.Generic;

public class FoodSource : MonoBehaviour
{
    public FoodType type;
    public float fullnessRestore;
    public bool deleteWhenEaten;
    public int bitesLeft;
    public float size;
    private List<FishController> fishes = new();
    public float Eat()
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
    public bool IsAvailable()
    {
        return bitesLeft - fishes.Count > 0;
    }
}
