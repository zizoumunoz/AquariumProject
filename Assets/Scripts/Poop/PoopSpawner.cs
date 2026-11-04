using UnityEngine;

public class PoopSpawner : MonoBehaviour
{
    [SerializeField] private Poop poopPrefab;
    [SerializeField] private Transform poopRoot;

    private void OnEnable()
    {
        PoopEmitter.PoopDropped += Spawn;
    }

    private void OnDisable()
    {
        PoopEmitter.PoopDropped -= Spawn;
    }

    private void Spawn(Vector3 position)
    {
        Instantiate(poopPrefab, position, Quaternion.identity, poopRoot);
    }
}