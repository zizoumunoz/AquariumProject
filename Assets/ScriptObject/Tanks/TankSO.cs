using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankSO", menuName = "Scriptable Objects/TankSO")]
public class TankSO : ScriptableObject
{
    public TankType type;
    public Sprite sprite;
    public TankAttribute startAttributes = new();
    public int maxFish = 8;
    public int price = 1;
}

[Serializable]
public class Tank
{
    public Tank()
    {
        type = TankType.Fresh;
        maxFish = 8;
    }
    public List<SaveFish> fishes = new();
    public List<SaveDecoration> decorations = new();
    public TankType type = TankType.Fresh;
    public TankAttribute currentAttibutes = new();
    public float maxFish = 8;

}

public enum TankType
{
    Fresh,
    Salty,
    Giant,
    Candy
}

[Serializable]
public class TankAttribute
{
    public TankAttribute()
    {

    }
    public TankAttribute(TankAttribute other)
    {
        oxigen = other.oxigen;
        temperature = other.temperature;
        moldLevel = other.moldLevel;
        pHLevel = other.pHLevel;
        salinity = other.salinity;
        light = other.light;
    }
    public float oxigen = 50;
    public float temperature = 50;
    public float moldLevel = 0;
    public float pHLevel = 50;
    public float salinity = 50;
    public float light = 50;
}