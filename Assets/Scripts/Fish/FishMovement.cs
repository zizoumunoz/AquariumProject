using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishMovement : MonoBehaviour, IFishBehaviour
{
    private Rigidbody2D rb;
    private FishStats stats;
    private FishBounds bounds;
    private float angle;
    [SerializeField] public Transform target;

    public int Order => 1;

    public void Init(FishStats stats)
    {
        rb = GetComponent<Rigidbody2D>();
        this.stats = stats;
        bounds = stats.GetBehaviour<FishBounds>();
    }
    public void UpdateFish()
    {
        transform.eulerAngles = Vector3.forward * angle;
        rb.linearVelocity = stats[StatsEnum.fishSpeed] * -transform.right;
        if (target == null)
        {
            return;
        }

        float targetAngle = Vector2.SignedAngle(-transform.right, target.position - transform.position);

        if (targetAngle > 4f)
        {
            TurnUp();
        }
        else if (targetAngle < -4f)
        {
            TurnDown();
        }
    }
    public void TurnUp()
    {
        AddAngle(stats[StatsEnum.turnSpeed]);
    }
    public void TurnDown()
    {
        AddAngle(-stats[StatsEnum.turnSpeed]);
    }
    private void AddAngle(float addAngle)
    {
        angle += addAngle * Time.deltaTime;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
