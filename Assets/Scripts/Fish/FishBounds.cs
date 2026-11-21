using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FishBounds : MonoBehaviour, IFishBehaviour
{
    private Collider2D col;
    private readonly Dictionary<Direction, Func<Transform, Vector2>> directions = new()
    {
        { Direction.up, x => x.up },
        { Direction.upFront, x => x.up - x.right },
        { Direction.front, x => -x.right },
        { Direction.downFront, x => -x.up - x.right },
        { Direction.down, x => -x.up },
        { Direction.downBack, x => -x.up + x.right },
        { Direction.back, x => x.right },
        { Direction.upBack, x => x.up + x.right }
    };
    [SerializeField] public Dictionary<Direction, float> distanceToWalls = new();
    public List<Collider2D> fishCollisions = new();
    public Collider2D[] fishFoodRadius;
    public List<Collider2D> mouthCollisions = new();

    public int Order => 0;

    public void Init(FishStats stats)
    {
        col = GetComponent<Collider2D>();

        UpdateFish();
    }

    public void UpdateFish()
    {
        for (Direction i = 0; i <= Direction.upBack; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directions[i](transform), float.MaxValue, LayerMask.GetMask("Wall"));

            float minD = float.MaxValue;
            for (int j = 0; j < hits.Length; j++)
            {
                float thisD = (hits[j].point - (Vector2)transform.position).magnitude;
                if (thisD < minD)
                {
                    minD = thisD;
                }
            }

            distanceToWalls[i] = minD;
        }
        fishFoodRadius = Physics2D.OverlapCircleAll(transform.position, 50f, LayerMask.GetMask("Fish", "Food"));
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