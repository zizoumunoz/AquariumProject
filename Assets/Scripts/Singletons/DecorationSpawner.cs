using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    Camera cam;
    public static DecorationSpawner instance;
    [SerializeField] private GameObject spawningDeco;
    [SerializeField] private DecorationSO spawningDecoSO;
    [SerializeField] private Transform decoRoot;
    [SerializeField] private BoolInput placed;
    [SerializeField] private Vector2Input mousePos;
    [SerializeField] private GameObject placingScreen;
    [SerializeField] private Rect placementArea = new Rect(-7f, -4f, 14f, 6f);

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        placed = InputManager.Instance.GetBoolInput("LeftClick");
        mousePos = InputManager.Instance.GetVector2Input("MousePosition");
        cam = Camera.main;
    }
    public void Spawn(DecorationSO so)
    {
        spawningDecoSO = so;
        spawningDeco = Instantiate(so.prefab, Vector2.zero, Quaternion.identity, decoRoot);
        spawningDeco.AddComponent<DecorationSOHolder>().so = so;
        foreach (MonoBehaviour component in spawningDeco.GetComponents<MonoBehaviour>())
        {
            component.enabled = true;
        }
        spawningDeco = null;
    }
    public void Spawn(DecorationSO so, Vector2 pos)
    {
        spawningDecoSO = so;
        spawningDeco = Instantiate(so.prefab, pos, Quaternion.identity, decoRoot);
        spawningDeco.AddComponent<DecorationSOHolder>().so = so;
        foreach (MonoBehaviour component in spawningDeco.GetComponents<MonoBehaviour>())
        {
            component.enabled = true;
        }
        spawningDeco = null;
    }
    public void DestroySpawningDeco()
    {
        Destroy(spawningDeco);
        ScreenManager.instance.PopScreen();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(placementArea.center, placementArea.size);
    }

    private void Update()
    {
        if (spawningDeco == null)
        {
            return;
        }

        Vector2 pos = cam.ScreenToWorldPoint(mousePos.value);
        if (spawningDecoSO.restrictPlacement) // if the restrict placement flag is on, clamp so you can't place too close to edge
        {
            float halfWidth = spawningDeco.GetComponent<SpriteRenderer>().bounds.extents.x;
            pos.x = Mathf.Clamp(pos.x, placementArea.xMin + halfWidth, placementArea.xMax - halfWidth);
            pos.y = Mathf.Clamp(pos.y, placementArea.yMin, placementArea.yMax);
        }

        spawningDeco.transform.position = pos;

        // spawningDeco.transform.position = cam.ScreenToWorldPoint(mousePos.value);
        // spawningDeco.transform.position = new Vector2(spawningDeco.transform.position.x, spawningDeco.transform.position.y);

        if (placed.pressed)
        {
            foreach (MonoBehaviour component in spawningDeco.GetComponents<MonoBehaviour>())
            {
                component.enabled = true;
            }
            spawningDeco = null;
            MoneyManager.Instance.SpendMoney(spawningDecoSO.price);
            ScreenManager.instance.PopScreen();
        }
    }
}
