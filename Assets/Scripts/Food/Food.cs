using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour
{
    [SerializeField] private FoodSO data;

    [Header("Sink")]
    [SerializeField] private float baseFallDelay = 3f;
    [SerializeField] private float sinkSpeed = 0.5f;

    [SerializeField] public float timeToDecay = 5;

    private Rigidbody2D rb;
    private float fallDelay;
    private bool sinking;
    private bool landed;

    public void Init(FoodSO so)
    {
        data = so;
        GetComponent<SpriteRenderer>().sprite = data.sprite;

        fallDelay = baseFallDelay / Mathf.Max(data.weight, 0.01f);
        sinking = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        FoodSource source = GetComponent<FoodSource>();
        source.fullnessRestore = data.fullnessRestore;
        source.type = data.foodType;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Init(data);
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
        rb.linearVelocity = data.weight * sinkSpeed * Vector2.down;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (landed) return;
        if (!collision.gameObject.CompareTag("Bottom")) return;

        landed = true;
        Destroy(gameObject, timeToDecay);
    }

}