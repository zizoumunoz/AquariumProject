using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Poop : MonoBehaviour
{
    [SerializeField] private float sinkSpeed = 0.5f;

    private Rigidbody2D rb;
    public bool landed;
    public float dissolveTimer = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        if (landed) return;
        rb.linearVelocity = Vector2.down * sinkSpeed;
    }

    private void Update()
    {
        dissolveTimer -= Time.deltaTime;
        if (dissolveTimer < 0f)
        {
            Dissolve();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (landed) return;
        if (!collision.gameObject.CompareTag("Bottom")) return;

        landed = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Consume()
    {
        Destroy(gameObject);
    }
    public void Dissolve()
    {
        TankManager.instance.database.activeTank.currentAttibutes.moldLevel +=
            Mathf.Min(100, TankManager.instance.database.activeTank.currentAttibutes.moldLevel + 10);
        Destroy(gameObject);
    }
}