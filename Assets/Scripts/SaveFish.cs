using System;
using System.Collections.Generic;

[Serializable]
public class SaveFish
{
    public FishSO baseFish;
    public List<TraitSO> traits;
    public float growTimer;
}