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
        ScreenManager.instance.EmptyStack();
        ScreenManager.instance.OpenScreen(placingScreen);
    }
    public void DestroySpawningDeco()
    {
        Destroy(spawningDeco);
        ScreenManager.instance.PopScreen();
    }
    private void Update()
    {
        if (spawningDeco == null)
        {
            return;
        }

        spawningDeco.transform.position = cam.ScreenToWorldPoint(mousePos.value);
        spawningDeco.transform.position = new Vector2(spawningDeco.transform.position.x, spawningDeco.transform.position.y);

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
