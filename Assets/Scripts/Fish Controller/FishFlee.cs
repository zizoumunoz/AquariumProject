using UnityEngine;


[RequireComponent(typeof(FishController), typeof(FishMovement))]
public class FishFlee : MonoBehaviour, IFishBehaviour
{
    private const int DirectionCount = 8;          
    private const float LookAhead = 4f;            
    private const float RepathInterval = 0.15f;    
    private const float ClearanceWeight = 1f;
    private const float PanicTurnMultiplier = 3f;


    private FishController fish;
    private FishMovement movement;
    private int wallMask;

    private FishController chaser;
    private float chasedTimer;

    private Vector2 fleeDirection;
    private float repathTimer;
    private float slideSide;

    public bool IsAfraid { get { return chaser != null; } }

    private void Awake()
    {
        fish = GetComponent<FishController>();
        movement = GetComponent<FishMovement>();
        wallMask = LayerMask.GetMask("Tank");
        slideSide = Random.value < 0.5f ? 1f : -1f;
    }
     
    public void NotifyChased(FishController predator)
    {
        if (chaser == null) repathTimer = 0f;

        chaser = predator;
        chasedTimer = fish.Data.fishStatus.fearMemoryTime * fish.statusMultiplier.fearMemoryTime;
    }

    private void Update()
    {
        if (chaser == null) return;

        chasedTimer -= Time.deltaTime;
        if (chasedTimer <= 0) chaser = null;
    }
    private float Score(Vector2 direction, Vector2 away, Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, LookAhead, wallMask);
        float clearance = hit.collider != null ? hit.distance / LookAhead : 1f;

        return Vector2.Dot(direction, away) + ClearanceWeight * clearance;
    }
    private Vector2 SlideAlongWall(Vector2 direction)
    {
        if (!movement.TouchingWall || Vector2.Dot(direction, movement.WallNormal) >= 0f) return direction;

        Vector2 normal = movement.WallNormal;
        Vector2 slide = direction - normal * Vector2.Dot(direction, normal);
        if (slide.sqrMagnitude < 0.05f)
            slide = new Vector2(-normal.y, normal.x) * slideSide;

        return slide.normalized;
    }
    private Vector2 ChooseDirection()
    {
        Vector2 pos = transform.position;
        Vector2 away = (pos - (Vector2)chaser.transform.position).normalized;

        Vector2 best = away;
        float bestScore = Score(away, away, pos);

        for (int i = 0; i < DirectionCount; i++)
        {
            float angle = i * (360f / DirectionCount) * Mathf.Deg2Rad;
            Vector2 candidate = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            float score = Score(candidate, away, pos);
            if (score > bestScore)
            {
                bestScore = score;
                best = candidate;
            }
        }
        return best;
    }


    public bool Tick()
    {
        if (chaser == null) return false;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            fleeDirection = ChooseDirection();
            repathTimer = RepathInterval;
        }

        Vector2 direction = SlideAlongWall(fleeDirection);
        Vector2 dest = (Vector2)transform.position + direction * fish.Data.fishStatus.fleeDistance * fish.statusMultiplier.fleeDistance;

        float maxSpeed = Mathf.Min(fish.CurrentSpeed, chaser.CurrentSpeed * fish.Data.fishStatus.fleeSpeedRatio * fish.statusMultiplier.fleeSpeedRatio);
        float facing = Mathf.Clamp01(Vector2.Dot(movement.Forward, direction));

        movement.MoveTo(dest, maxSpeed * facing, PanicTurnMultiplier);
        return true;
    }

    



}
