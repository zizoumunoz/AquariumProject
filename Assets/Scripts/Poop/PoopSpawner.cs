using UnityEngine;

public class PoopSpawner : MonoBehaviour
{
    public static PoopSpawner instance;
    [SerializeField] private Poop poopPrefab;
    [SerializeField] private Transform poopRoot;

    private void Awake()
    {
        instance = this;
    }
    public void Spawn(Vector3 position)
    {
        Instantiate(poopPrefab, position, Quaternion.identity, poopRoot);
    }
}