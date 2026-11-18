using UnityEngine;

public class TankBounds : MonoBehaviour
{
    public static TankBounds Instance { get; private set; }

    [SerializeField] private Collider2D floor;
    [SerializeField] private Collider2D ceiling;
    [SerializeField] private Collider2D leftWall;
    [SerializeField] private Collider2D rightWall;

    public Vector2 Min { get { return new Vector2(leftWall.bounds.max.x, floor.bounds.max.y); } }
    public Vector2 Max { get { return new Vector2(rightWall.bounds.min.x, ceiling.bounds.min.y); } }

    private void Awake()
    {
        Instance = this;
    }
}