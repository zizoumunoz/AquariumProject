using UnityEngine;


[RequireComponent(typeof(FishController), typeof(FishMovement))]
public class FishWander : MonoBehaviour, IFishBehaviour
{
    private const float lookAhead = 5.0f;

    private FishController fish;
    private FishMovement movement;

    private Vector2 direction;
    private float retargetTimer;

    private void Awake()
    {
        fish = GetComponent<FishController>();
        movement = GetComponent<FishMovement>();
    }

    public bool Tick()
    {
        retargetTimer -= Time.deltaTime;

        bool blocked = movement.TouchingWall && Vector2.Dot(direction, movement.WallNormal) < 0.0f;
        if (retargetTimer <= 0.0f || blocked) PickNewDirection();

        movement.MoveTo((Vector2)transform.position + direction * lookAhead, fish.CurrentSpeed);
        return true;
    }

    private void PickNewDirection()
    {
        float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (movement.TouchingWall && Vector2.Dot(dir, movement.WallNormal) < 0f)
        {
            dir = Vector2.Reflect(dir,movement.WallNormal);
        }

        direction = dir;

        retargetTimer = Random.Range(fish.Data.fishStatus.minRetargetTime * fish.statusMultiplier.minRetargetTime,fish.Data.fishStatus.maxRetargetTime * fish.statusMultiplier.maxRetargetTime);
    }

}
