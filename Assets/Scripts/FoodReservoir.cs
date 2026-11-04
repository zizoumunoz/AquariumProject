using System;
using UnityEngine;

public class FoodReservoir : MonoBehaviour
{
    public event Action Depleted;
    public event Action Refilled;

    private int capacity;
    private float reloadTime;
    private int bitesLeft;
    private float reloadTimer;

    public bool IsReloading { get; private set; }

    public float ReloadProgress
    {
        get { return IsReloading && reloadTime > 0f ? 1f - reloadTimer / reloadTime : 1f; }
    }

    public void Configure(int newCapacity, float newReloadTime)
    {
        capacity = newCapacity;
        reloadTime = newReloadTime;
        bitesLeft = capacity;
        IsReloading = false;
    }

    public bool TryTake()
    {
        if (IsReloading || bitesLeft <= 0) return false;

        bitesLeft--;
        if (bitesLeft == 0) StartReload();
        return true;
    }

    private void Update()
    {
        if (!IsReloading) return;

        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0f) Refill();
    }

    private void StartReload()
    {
        IsReloading = true;
        reloadTimer = reloadTime;
        Depleted?.Invoke();
    }

    private void Refill()
    {
        IsReloading = false;
        bitesLeft = capacity;
        Refilled?.Invoke();
    }
}