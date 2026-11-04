using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationSO", menuName = "Scriptable Objects/DecorationSO")]
public class DecorationSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public float[] currentAttibutes = new float[Enum.GetValues(typeof(TankAttribute)).Length];
    public string description;
    public int price = 100;
}
