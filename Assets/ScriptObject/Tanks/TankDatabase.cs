using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankDatabase", menuName = "Scriptable Objects/TankDatabase")]
public class TankDatabase : ScriptableObject
{
    public List<TankSO> tankSOs = new();
    public List<Tank> tanks = new() { new Tank() };
    public Tank activeTank = null;

    private void OnValidate()
    {
        tankSOs.Sort((x, y) => x.type - y.type);
    }
}