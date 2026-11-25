using System;
using System.Collections.Generic;

[Serializable]
public class SaveFish
{
    public int baseFish;
    public TankType tank;
    public List<int> traits;
    public float growTimer;
}