using System;
using UnityEngine;

[RequireComponent(typeof(FishController))]
public class PoopEmitter : MonoBehaviour
{
    public static event Action<Vector3> PoopDropped;

    private FishController fish;
    private float elapsed;

    private void Awake()
    {
        fish = GetComponent<FishController>();
    }

    private void Update()
    {
        if (fish.Data == null || fish.IsHungry) return;

        elapsed += Time.deltaTime;
        if (elapsed < fish.PoopInterval) return;

        elapsed = 0f;
        PoopDropped?.Invoke(transform.position);
    }
}
