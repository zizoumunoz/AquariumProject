using UnityEngine;


[RequireComponent(typeof(FishController), typeof(FishMovement))]
public class FishFlee : MonoBehaviour, IFishBehaviour
{
    private FishController fish;
    private FishMovement movement;
    private FishController chaser;
    private float chasedTimer;
    private float slideSide;

    public bool IsAfraid { get { return chaser != null; } }

    private void Awake()
    {
        fish = GetComponent<FishController>();
        movement = GetComponent<FishMovement>();
        slideSide = Random.value < 0.5f ? 1f : -1f;
    }

    public void NotifyChased(FishController predator)
    {
        chaser = predator;
        chasedTimer = fish.Data.fishStatus.fearMemoryTime * fish.statusMultiplier.fearMemoryTime;
    }

    private void Update()
    {
        if (chaser == null) return;

        chasedTimer -= Time.deltaTime;
        if (chasedTimer <= 0) chaser = null;
    }

    public bool Tick()
    {
        if (chaser == null) return false;

        Vector2 pos = transform.position;
        Vector2 away = (pos - (Vector2)chaser.transform.position).normalized;

        if (movement.TouchingWall && Vector2.Dot(away, movement.WallNormal) < 0f)
        {
            Vector2 normal = movement.WallNormal;
            Vector2 slide = away - normal * Vector2.Dot(away, normal);
            if (slide.sqrMagnitude < 0.05f)
                slide = new Vector2(-normal.y, normal.x) * slideSide;
            away = slide.normalized;
        }

        Vector2 dest = pos + away * fish.Data.fishStatus.fleeDistance * fish.statusMultiplier.fleeDistance;

        float speed = Mathf.Min(fish.CurrentSpeed, chaser.CurrentSpeed * fish.Data.fishStatus.fleeSpeedRatio * fish.statusMultiplier.fleeSpeedRatio);

        movement.MoveTo(dest, speed);
        return true;
    }

}
