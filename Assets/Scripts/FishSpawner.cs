using UnityEngine;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner instance;
    [SerializeField] private FishController fishPrefab;
    [SerializeField] private Transform fishRoot;

    private void Awake()
    {
        instance = this;
    }
    public void Spawn(FishSO so)
    {
        if (fishRoot.childCount == TankManager.instance.database.activeTank.maxFish)
        {
            return;
        }
        FishController fish = Instantiate(fishPrefab, Vector2.zero, Quaternion.identity, fishRoot);
        fish.Init(so, new FishStatus(1));
    }
    public void SpawnChild(FishSO so, FishStatus newStatus, List<TraitSO> traits, Vector2 pos)
    {
        if (fishRoot.childCount == TankManager.instance.database.activeTank.maxFish)
        {
            return;
        }
        FishController fish = Instantiate(fishPrefab, pos, Quaternion.identity, fishRoot);
        fish.Init(so, newStatus, traits);
    }
}