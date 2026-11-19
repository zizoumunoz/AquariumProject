using System;
using UnityEngine;

[RequireComponent(typeof(FishController))]
public class PoopEmitter : MonoBehaviour
{
    private FishStats stats;
    private float elapsed;

    private void Awake()
    {
        stats = GetComponent<FishStats>();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed < stats[StatsEnum.poopInterval]) return;

        elapsed = 0f;
        PoopSpawner.instance.Spawn(transform.position);
    }
}
