using System;
using System.Collections.Generic;

[Serializable]
public class SaveFish
{
    public FishSO baseFish;
    public FishStatus multiplier;
    public List<TraitSO> traits;
    public float growTimer;
}