using UnityEngine;

[CreateAssetMenu(fileName = "KelpSO", menuName = "Scriptable Objects/KelpSO")]
public class KelpSO : ScriptableObject
{
    [Header("Food")]
    public float fullnessPerBite = 15f;
    public int bitesBeforeReload = 8;
    public float reloadTime = 30f;
}