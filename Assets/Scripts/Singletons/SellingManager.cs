using UnityEngine;

public class SellingManager : MonoBehaviour
{
    [SerializeField] private BoolInput click;
    [SerializeField] private Vector2Input mousePosition;

    private void Start()
    {
        click = InputManager.Instance.GetBoolInput("RightClick");
        mousePosition = InputManager.Instance.GetVector2Input("MousePosition");
    }
    // Update is called once per frame
    void Update()
    {
        if (click.pressed)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition.value);
            pos.z = 0;
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.5f, LayerMask.GetMask("Fish"));

            if (hits.Length > 0)
            {
                FishController fish = hits[0].GetComponent<FishController>();
                MoneyDisplay.Instance.AddMoney(fish.data.price * 2);

                Destroy(hits[0].gameObject);
            }
        }
    }
}
