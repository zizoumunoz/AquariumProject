using UnityEngine;

[RequireComponent(typeof(GroundWalker))]
public class SnailController : MonoBehaviour
{
    private const float arriveDistance = 0.05f;

    [Header("Eating")]
    [SerializeField] private float sightRange = 2.5f;
    [SerializeField] private float eatDistance = 0.15f;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float minRetargetTime = 4f;
    [SerializeField] private float maxRetargetTime = 10f;

    private GroundWalker walker;
    private float wanderTargetX;
    private float retargetTimer;

    private void Awake()
    {
        walker = GetComponent<GroundWalker>();
    }

    private void Start()
    {
        PickWanderTarget();
    }

    private void Update()
    {
        Poop poop = FindNearestPoop();

        if (poop != null) GoEat(poop);
        else Wander();
    }

    private Poop FindNearestPoop()
    {
        Poop nearest = null;
        float best = sightRange;

        Collider2D[] shits = Physics2D.OverlapCircleAll(transform.position, 50f, LayerMask.GetMask("Poop"));
        foreach (Collider2D shit in shits)
        {
            Poop poop = shit.GetComponent<Poop>();
            if (!poop.landed) continue;

            float d = Vector2.Distance(transform.position, poop.transform.position);
            if (d > best) continue;

            best = d;
            nearest = poop;
        }
        return nearest;
    }

    private void GoEat(Poop poop)
    {
        float poopX = poop.transform.position.x;

        if (Mathf.Abs(poopX - walker.X) <= eatDistance)
        {
            poop.Consume();
            return;
        }

        walker.MoveTowards(poopX);
    }

    private void Wander()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f || Mathf.Abs(wanderTargetX - walker.X) < arriveDistance)
            PickWanderTarget();

        walker.MoveTowards(wanderTargetX);
    }

    private void PickWanderTarget()
    {
        wanderTargetX = walker.ClampToScreen(walker.X + Random.Range(-wanderRadius, wanderRadius));
        retargetTimer = Random.Range(minRetargetTime, maxRetargetTime);
    }
}