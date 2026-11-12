using System;
using UnityEngine;

[RequireComponent(typeof(FishController))]
public class PoopEmitter : MonoBehaviour
{
    private FishController fish;
    private float elapsed;

    private void Awake()
    {
        fish = GetComponent<FishController>();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed < fish.PoopInterval) return;

        elapsed = 0f;
        PoopSpawner.instance.Spawn(transform.position);
    }
}
