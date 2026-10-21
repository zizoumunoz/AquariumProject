using UnityEngine;

public enum FoodType
{
    Plant,
    Meat
}


[CreateAssetMenu(fileName = "FoodSO", menuName = "Scriptable Objects/FoodSO")]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public Sprite sprite;
    public FoodType foodType;
    public float fullnessRestore = 30f;
    public float weight = 1; // 1 is normal!
    public float price = 10;

}