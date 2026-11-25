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

    // event for when food is eaten
    public event System.Action<FoodSource> Eaten;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public float Eat()
    {
        bitesLeft--;
        Eaten?.Invoke(this);
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
    public float DistanceTo(Collider2D body)
    {
        if (col == null) return Vector2.Distance(body.bounds.center, transform.position);
        return Mathf.Max(0.0f, body.Distance(col).distance);
    }

}
