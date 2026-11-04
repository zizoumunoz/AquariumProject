using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WallSnap : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.left;
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private float searchDistance = 30f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Snap();
    }

    public void PlaceAtHeight(float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
        Snap();
    }

    private void Snap()
    {
        float edgeX = direction.x < 0f ? sr.bounds.min.x : sr.bounds.max.x;
        float edgeOffset = transform.position.x - edgeX;

        RaycastHit2D[] hits = Physics2D.RaycastAll(sr.bounds.center, direction, searchDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag(wallTag)) continue;

            Vector3 pos = transform.position;
            pos.x = hit.point.x + edgeOffset;
            transform.position = pos;
            return;
        }

        Debug.LogWarning(name + " found no '" + wallTag + "' collider in direction " + direction + ".", this);
    }
}