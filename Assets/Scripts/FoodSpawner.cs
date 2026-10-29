using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] Food foodPrefab;
    [SerializeField] FoodSO foodToSpawn;
    [SerializeField] private Transform foodRoot;
    public float topOffset = 0.5f;
    [SerializeField] private BoolInput click;
    [SerializeField] private Vector2Input mousePosition;

    private void Start()
    {
        click = InputManager.Instance.GetBoolInput("LeftClick");
        mousePosition = InputManager.Instance.GetVector2Input("MousePosition");
    }
    // Update is called once per frame
    void Update()
    {
        if (click.pressed && ScreenManager.instance.Empty())
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition.value);

            Camera cam = Camera.main;
            pos.y = cam.transform.position.y + cam.orthographicSize - topOffset;
            pos.z = 0f;
            Food newFood = Instantiate(foodPrefab, foodRoot);
            newFood.transform.position = pos;
            newFood.Init(foodToSpawn);
        }
    }
}
