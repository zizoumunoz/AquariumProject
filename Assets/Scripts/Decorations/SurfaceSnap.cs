using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SurfaceSnap : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.down;
    [SerializeField] private string surfaceTag = "Bottom";
    [SerializeField] private float searchDistance = 40f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Snap();
    }

    public void Snap()
    {
        bool horizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        Vector2 axis = horizontal ? new Vector2(Mathf.Sign(direction.x), 0f) : new Vector2(0f, Mathf.Sign(direction.y));

        Bounds bounds = sr.bounds;
        Vector2 facingEdge = (Vector2)bounds.center + Vector2.Scale(axis, bounds.extents);
        Vector2 edgeOffset = (Vector2)transform.position - facingEdge;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, axis, searchDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag(surfaceTag)) continue;

            Vector3 pos = transform.position;
            if (horizontal) pos.x = hit.point.x + edgeOffset.x;
            else pos.y = hit.point.y + edgeOffset.y;

            transform.position = pos;
            return;
        }

        Debug.Log(name + " found no " + surfaceTag + " collider in direction " + axis + ".", this);
    }
}
