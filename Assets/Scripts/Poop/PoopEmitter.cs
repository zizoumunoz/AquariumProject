using System;
using UnityEngine;

[RequireComponent(typeof(FishController))]
public class PoopEmitter : MonoBehaviour, IFishBehaviour
{
    private FishStats stats;
    private float elapsed;

    public int Order => 4;

    public void Init(FishStats stats)
    {
        this.stats = stats;
    }

    public void UpdateFish()
    {
        elapsed += Time.deltaTime;
        if (elapsed < stats[StatsEnum.poopInterval]) return;

        elapsed = 0f;
        PoopSpawner.instance.Spawn(transform.position);
    }
}
