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
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

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
    public float DistanceTo(Vector2 from)
    {
        if (col == null) return Vector2.Distance(from, transform.position);
        if (col.OverlapPoint(from)) return 0f;
        return Vector2.Distance(from, col.ClosestPoint(from));
    }

}
