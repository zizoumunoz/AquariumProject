using System;
using System.Collections.Generic;
using UnityEngine;

public class FishCollection : MonoBehaviour
{
    private static readonly HashSet<FishSO> discovered = new();
    public static event Action<FishSO> FishDiscovered;
    public static bool IsDiscovered(FishSO fish)
    {
        return fish != null && discovered.Contains(fish);
    }

    public static void Discover(FishSO fish)
    {
        if (fish == null || !discovered.Add(fish))
        {
            return;
        }
        FishDiscovered?.Invoke(fish);
    }

    public static void Clear()
    {
        discovered.Clear();
    }

    // Reset list when game starts
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        discovered.Clear();
        FishDiscovered = null;
    }
}
