using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour, IFoodSource
{
    [SerializeField] private FoodSO data;

    [Header("Sink")]
    [SerializeField] private float baseFallDelay = 3f;
    [SerializeField] private float sinkSpeed = 0.5f;

    [SerializeField]
    public float timeToDecay = 5;

    private Rigidbody2D rb;
    private float fallDelay;
    private bool sinking;
    private bool landed;

    public float FullnessRestore { get { return data.fullnessRestore; } }
    public FoodType Type { get { return data.foodType; } }
    public float Weight { get { return data.weight; } }

    public bool IsAvailable { get { return true; } }
    public Vector2 Position { get { return transform.position; } }

    public float Eat()
    {
        Destroy(gameObject);
        return FullnessRestore;
    }

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

    private void OnEnable()
    {
        FoodSources.Register(this);
    }

    private void OnDisable()
    {
        FoodSources.Unregister(this);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (landed) return;
        if (!collision.gameObject.CompareTag("Bottom")) return;

        landed = true;
        Destroy(gameObject, timeToDecay);
    }

}