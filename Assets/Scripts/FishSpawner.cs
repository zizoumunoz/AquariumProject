using UnityEngine;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner instance;
    [SerializeField] private FishController fishPrefab;
    [SerializeField] private Transform fishRoot;
    [SerializeField] private FishDatabaseSO database;
    [SerializeField] private TraitRules traitRules;

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
        fish.Init(so, new FishStatus(1), traitRules.RollPurchase(database));
    }
    public void SpawnChild(FishSO so, FishStatus newStatus, List<TraitSO> parentATraits, List<TraitSO> parentBTraits, Vector2 pos)
    {
        if (fishRoot.childCount == TankManager.instance.database.activeTank.maxFish)
        {
            return;
        }
        FishController fish = Instantiate(fishPrefab, pos, Quaternion.identity, fishRoot);
        fish.Init(so, newStatus, traitRules.RollChild(parentATraits, parentBTraits, database));
    }
}