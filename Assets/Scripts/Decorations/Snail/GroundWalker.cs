using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GroundWalker : MonoBehaviour, IMotionSource
{
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private float edgeMargin = 0.5f;
    [SerializeField] private bool spriteFacesRight = true;

    [Header("Ground")]
    [SerializeField] private string groundTag = "Bottom";
    [SerializeField] private float groundSearchDistance = 40f;

    private Camera cam;
    private SpriteRenderer sr;
    private bool moving;

    public float X { get { return transform.position.x; } }
    public float NormalizedSpeed { get { return moving ? 1f : 0f; } }

    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SnapToGround();
    }

    public float ClampToScreen(float x)
    {
        float halfWidth = cam.orthographicSize * cam.aspect;
        float centerX = cam.transform.position.x;
        return Mathf.Clamp(x, centerX - halfWidth + edgeMargin, centerX + halfWidth - edgeMargin);
    }

    public void MoveTowards(float targetX)
    {
        float distance = targetX - X;
        moving = Mathf.Abs(distance) > 0.001f;
        if (!moving) return;

        float direction = Mathf.Sign(distance);
        float step = Mathf.Min(speed * Time.deltaTime, Mathf.Abs(distance));

        Vector3 pos = transform.position;
        pos.x = ClampToScreen(pos.x + direction * step);
        transform.position = pos;

        sr.flipX = (direction > 0f) != spriteFacesRight;
    }

    private void SnapToGround()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, groundSearchDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag(groundTag)) continue;

            Vector3 pos = transform.position;
            pos.y = hit.point.y + transform.position.y - sr.bounds.min.y;
            transform.position = pos;
            return;
        }

        Debug.LogWarning(name + " found no '" + groundTag + "' collider below it.", this);
    }
}