using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner instance;
    [SerializeField] Food foodPrefab;
    [SerializeField] FoodSO foodToSpawn;
    [SerializeField] private Transform foodRoot;
    public float topOffset = 0.5f;
    [SerializeField] private Vector2Input mousePosition;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mousePosition = InputManager.Instance.GetVector2Input("MousePosition");
    }
    public void SpawnFood()
    {
        Camera cam = Camera.main;
        Vector2 pos = cam.ScreenToWorldPoint(mousePosition.value);
        pos.y = cam.transform.position.y + cam.orthographicSize - topOffset;
        Food newFood = Instantiate(foodPrefab, foodRoot);
        newFood.transform.position = pos;
        newFood.Init(foodToSpawn);
    }
}
