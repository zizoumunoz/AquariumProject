using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour
{
    [SerializeField] private FoodSO data;

    [Header("Sink")]
    [SerializeField] private float baseFallDelay = 3f;
    [SerializeField] private float sinkSpeed = 0.5f;

    private Rigidbody2D rb;
    private float fallDelay;
    private bool sinking;

    public float FullnessRestore { get { return data.fullnessRestore; } }
    public FoodType Type { get { return data.foodType; } }
    public float Weight { get { return data.weight; } }

    public void Init(FoodSO so)
    {
        data = so;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = data.sprite;

        fallDelay = baseFallDelay / Mathf.Max(data.weight, 0.01f);
        sinking = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (data != null) Init(data);
    }

    private void Update()
    {
        if (data == null || sinking) return;
        if (data.weight < 1f) return;

        fallDelay -= Time.deltaTime;
        if (fallDelay > 0f) return;

        sinking = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        if (!sinking) return;
        rb.linearVelocity = Vector2.down * sinkSpeed * data.weight;
    }
}