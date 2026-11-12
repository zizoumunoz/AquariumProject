using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationSO", menuName = "Scriptable Objects/DecorationSO")]
public class DecorationSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public TankAttribute currentAttibutes = new();
    public string description;
    public int price = 100;
}
