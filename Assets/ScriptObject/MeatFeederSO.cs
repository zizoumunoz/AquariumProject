using UnityEngine;

[CreateAssetMenu(fileName = "MeatFeederSO", menuName = "Scriptable Objects/MeatFeederSO")]
public class MeatFeederSO : ScriptableObject
{
    [Header("Food")]
    public FoodSO food;

    [Header("Dispensing")]
    public float dropInterval = 5f;
    public int dropsBeforeReload = 1;
    public float reloadTime = 30f;
}