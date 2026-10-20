using UnityEngine;

[CreateAssetMenu(fileName = "FoodSO", menuName = "Scriptable Objects/FoodSO")]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public Sprite sprite;
    public float fullnessRestore = 30f;
}