using System;
using UnityEngine;

[RequireComponent(typeof(FishController))]
public class PoopEmitter : MonoBehaviour
{
    private FishController fish;
    [SerializeField] private float elapsed;

    private void Awake()
    {
        fish = GetComponent<FishController>();
    }

    private void Update()
    {
        elapsed -= Time.deltaTime;
        if (elapsed > 0) return;

        elapsed = fish.PoopInterval;
        PoopSpawner.instance.Spawn(transform.position);
    }
}
