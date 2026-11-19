using System;
using System.Collections.Generic;

[Serializable]
public class SaveFish
{
    public FishSO baseFish;
#pragma warning disable UAC1015 // Dictionary field missing [SerializeField]
    public Dictionary<StatsEnum, float> statsMultiplier = new();
#pragma warning restore UAC1015 // Dictionary field missing [SerializeField]
    public List<TraitSO> traits;
    public float growTimer;
}