using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private MarketManager market;
    [SerializeField] private FishController fishPrefab;
    [SerializeField] private Transform fishRoot;

    private void OnEnable()
    {
        market.FishPurchased += Spawn;
    }

    private void OnDisable()
    {
        market.FishPurchased -= Spawn;
    }

    private void Spawn(FishSO so)
    {
        Vector3 pos = Camera.main.transform.position;
        pos.z = 0f;

        FishController fish = Instantiate(fishPrefab, pos, Quaternion.identity, fishRoot);
        fish.Init(so);
    }
}