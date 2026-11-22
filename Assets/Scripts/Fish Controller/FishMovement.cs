using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class FishMovement : MonoBehaviour, IMotionSource
{
    private const float WallContactMemory = 0.1f;

    [SerializeField] private float turnSpeed = 120f;
    private bool spriteFacesLeft = true;

    private FishController fish;
    private Rigidbody2D rb;
    private CapsuleCollider2D body;

    private Vector2 destination;
    private float speed;

    private float heading;
    private float normalizedSpeed;
    private bool ready;

    private Vector2 wallNormal;
    private float wallContactTime = -1f;

    public float NormalizedSpeed { get { return normalizedSpeed; } }
    public Collider2D Body { get { return body; } }
    public bool TouchingWall { get { return Time.time - wallContactTime < WallContactMemory; } }

    public Vector2 WallNormal { get { return wallNormal; } }

    private void Awake()
    {
        fish = GetComponent<FishController>();
        rb = GetComponent<Rigidbody2D>();
        body = GetComponent<CapsuleCollider2D>();
    }

    public void Setup(Sprite sprite)
    {
        FitColliderToSprite(sprite);
        heading = Random.Range(0f, 360f);
        speed = 0f;
        ready = true;
    }

    private void FitColliderToSprite(Sprite sprite)
    {
        Bounds local = sprite.bounds; 
        body.offset = local.center; 
        body.size = local.size;
        body.direction = local.size.x >= local.size.y ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;
    }

    public void MoveTo(Vector2 dest, float moveSpeed)
    {
        destination = dest; 
        speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (!ready) return;

        Vector2 toTarget = destination - rb.position;

        float desiredAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        float diff = Mathf.DeltaAngle(heading, desiredAngle);
        heading = Mathf.MoveTowardsAngle(heading, desiredAngle, turnSpeed * Time.fixedDeltaTime);

        float speedFactor = Mathf.Lerp(0.4f, 1.0f, Mathf.Clamp01(Mathf.Cos(diff * Mathf.Deg2Rad) * 0.5f + 0.5f));
        float maxSpeed = fish.CurrentSpeed;
        normalizedSpeed = maxSpeed > 0.0f ? Mathf.Clamp01(speed * speedFactor / maxSpeed) : 0.0f;

        float headingRad = heading * Mathf.Deg2Rad;
        Vector2 forward = new Vector2(Mathf.Cos(headingRad), Mathf.Sin(headingRad));

        rb.linearVelocity = forward * speed * speedFactor;
        rb.MoveRotation(heading - (spriteFacesLeft ? 180 : 0f));

        bool flip = spriteFacesLeft ? forward.x > 0.0f : forward.x < 0.0f;
        fish.Visual.flipY = flip;
        fish.highlight.flipY = flip;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RecordWall(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        RecordWall(collision);
    }

    private void RecordWall(Collision2D colision)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < colision.contactCount; i++)
        {
            sum += colision.GetContact(i).normal;
        }

        if (sum.sqrMagnitude < 0.0001f) return;

        wallNormal = sum.normalized;
        wallContactTime = Time.time;
    }


}
