using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankDatabase", menuName = "Scriptable Objects/TankDatabase")]
public class TankDatabase : ScriptableObject
{
    public List<Tank> tanks = new() { new Tank() };
    public Tank activeTank;
    public Sprite[] backgrounds = new Sprite[Enum.GetValues(typeof(TankType)).Length];
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
    public List<SaveFish> fishes;
    public List<DecorationSO> decorations;
    public TankType type = TankType.Fresh;
    public float[] currentAttibutes = new float[Enum.GetValues(typeof(TankAttribute)).Length];
    public float maxFish = 8;

}

[Serializable]
public class SaveFish
{
    public FishSO baseFish;
    public FishStatus multiplier;
    public List<TraitSO> traits;
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