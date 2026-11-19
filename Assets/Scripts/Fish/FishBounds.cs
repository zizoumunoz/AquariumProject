using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FishBounds : MonoBehaviour, IFishBehaviour
{
    private Collider2D col;
    private readonly Dictionary<Direction, Vector2> directions = new()
    {
        { Direction.up, Vector2.up },
        { Direction.upFront, Vector2.up + Vector2.right },
        { Direction.front, Vector2.right },
        { Direction.downFront, Vector2.down + Vector2.right },
        { Direction.down, Vector2.down },
        { Direction.downBack, Vector2.down + Vector2.left },
        { Direction.back, Vector2.left },
        { Direction.upBack, Vector2.up + Vector2.left }
    };
    [SerializeField] public Dictionary<Direction, RaycastHit2D[]> rays = new();
    public List<Collider2D> fishCollisions = new();
    public List<Collider2D> fishRadius = new();
    public List<Collider2D> mouthCollisions = new();
    public void Init(FishStats stats)
    {
        col = GetComponent<Collider2D>();
    }

    public void UpdateFish()
    {
        for (Direction i = 0; i <= Direction.upBack; i++)
        {
            rays[i] = Physics2D.RaycastAll(transform.position, directions[i], float.MaxValue, LayerMask.GetMask("Wall"));
        }
        fishRadius = Physics2D.OverlapCircleAll(transform.position, 5, LayerMask.GetMask("Fish", "Food")).Where(x => x != col).ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fishCollisions.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        fishCollisions.Remove(collision);
    }
}

public enum Direction
{
    up,
    upFront,
    front,
    downFront,
    down,
    downBack,
    back,
    upBack
}