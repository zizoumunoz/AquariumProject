using UnityEngine;

public interface IFoodSource
{
    FoodType Type { get; }
    bool IsAvailable { get; }
    Vector2 Position { get; }
    float Eat();
}
