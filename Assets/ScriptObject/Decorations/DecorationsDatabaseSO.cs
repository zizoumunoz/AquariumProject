using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationsDatabaseSO", menuName = "DecorationsDatabaseSO")]
public class DecorationsDatabaseSO : ScriptableObject
{
    public static DecorationsDatabaseSO instance;
    public List<DecorationSO> decorations = new();
    public DecorationSO GetByName(string name)
    {
        return decorations.Find(f => f.name == name);
    }

    public DecorationSO GetRandom()
    {
        return decorations[Random.Range(0, decorations.Count)];
    }
    private void Awake()
    {
        instance = this;
    }
}
