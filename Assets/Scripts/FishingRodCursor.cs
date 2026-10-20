using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class FishingRodCursor : MonoBehaviour
{
    [Header("Frames")]
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] castFrames;

    [Header("Rod")]
    [SerializeField] private Vector2 hotspotPixels = new Vector2(107f, 84f);
    [SerializeField] private int landCastIndex = 7;

    [Header("Food")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private FoodSO foodData;

    private static readonly float[] IdleDurations = { 0.40f, 0.40f };
    private static readonly float[] CastDurations = { 0.05f, 0.05f, 0.10f, 0.04f, 0.04f, 0.06f, 0.05f, 0.06f, 0.07f, 0.08f };

    private Camera cam;
    private SpriteRenderer sr;
    private int frame;
    private float timer;
    private bool casting;
    private bool pendingLand;
    private Vector2 landPos;

    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 100;
    }

    private void Start()
    {
        Cursor.visible = false;
        ShowFrame();
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    private Vector2 GetMouseWorld()
    {
#if ENABLE_INPUT_SYSTEM
        Vector3 m = Mouse.current.position.ReadValue();
#else
        Vector3 m = Input.mousePosition;
#endif
        m.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(m);
    }

    private bool ClickedThisFrame()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    private void Update()
    {
        Vector2 mouse = GetMouseWorld();

        if (ClickedThisFrame()) Cast(mouse);

        AdvanceAnimation();
        ShowFrame();
        PlaceOnMouse(mouse);
    }

    private void Cast(Vector2 pos)
    {
        if (pendingLand) SpawnFood(landPos);

        landPos = pos;
        pendingLand = true;
        casting = true;
        frame = 0;
        timer = 0f;
    }

    private void AdvanceAnimation()
    {
        timer += Time.deltaTime;

        while (true)
        {
            float dur = casting ? CastDurations[frame] : IdleDurations[frame];
            if (timer < dur) break;
            timer -= dur;

            if (!casting)
            {
                frame = (frame + 1) % idleFrames.Length;
                continue;
            }

            frame++;
            if (frame == landCastIndex && pendingLand)
            {
                SpawnFood(landPos);
                pendingLand = false;
            }
            if (frame >= castFrames.Length)
            {
                casting = false;
                frame = 0;
            }
        }
    }

    private void ShowFrame()
    {
        sr.sprite = casting ? castFrames[frame] : idleFrames[frame];
    }

    private void PlaceOnMouse(Vector2 mouse)
    {
        Sprite s = sr.sprite;
        if (s == null) return;

        float hx = hotspotPixels.x - s.pivot.x;
        float hy = (s.rect.height - hotspotPixels.y) - s.pivot.y;
        Vector2 offset = new Vector2(hx, hy) / s.pixelsPerUnit;
        offset.x *= transform.lossyScale.x;
        offset.y *= transform.lossyScale.y;

        transform.position = new Vector3(mouse.x - offset.x, mouse.y - offset.y, 0f);
    }

    private void SpawnFood(Vector2 pos)
    {
        if (foodPrefab == null) return;

        GameObject go = Instantiate(foodPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        Food food = go.GetComponent<Food>();
        if (food != null && foodData != null) food.Init(foodData);
    }
}