using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankSO", menuName = "Scriptable Objects/TankSO")]
public class TankSO : ScriptableObject
{
    public TankType type;
    public Sprite sprite;
    public float[] startAttributes = new float[Enum.GetValues(typeof(TankAttribute)).Length];
    public int maxFish = 8;
    public int price = 1;
}

[Serializable]
public class Tank
{
    public Tank()
    {
        type = TankType.Fresh;
        currentAttibutes = new float[Enum.GetValues(typeof(TankAttribute)).Length];
        maxFish = 8;
    }
    public List<SaveFish> fishes = new();
    public List<SaveDecoration> decorations = new();
    public TankType type = TankType.Fresh;
    public float[] currentAttibutes = new float[Enum.GetValues(typeof(TankAttribute)).Length];
    public float maxFish = 8;

}

public enum TankType
{
    Fresh,
    Salty,
    Giant,
    Candy
}

public enum TankAttribute
{
    Oxygen,
    Temperature,
    MoldLevel,
    PHLevel,
    Salinity,
    Light
}