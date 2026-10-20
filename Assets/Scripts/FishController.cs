using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static readonly List<FishController> All = new List<FishController>();

    [SerializeField] private FishSO data;
    [SerializeField] private float boundsMargin = 0.5f;

    [Header("Direction")]
    [SerializeField] private bool spriteFacesLeft = true;
    [SerializeField] private float turnSpeed = 120f;

    [Header("Flee")]
    [SerializeField] private float fleeSpeedRatio = 0.9f;
    [SerializeField] private float fleeDistance = 5f;
    [SerializeField] private float fearMemoryTime = 0.5f;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Icon")]
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private StatusIconSO statusIcons;
    [SerializeField] private float iconPadding = 0.15f;
    [SerializeField] private float iconSize = 1f;

    private Camera cam;
    private SpriteRenderer sr;
    private Vector2 min, max;
    private Vector2 target;
    private float retargetTimer;
    private float fullness;
    private float heading;

    private float sizeMult = 1f;
    private float speedMult = 1f;
    private float turnMult = 1f;
    private float hungerMult = 1f;
    private List<TraitSO> traits = new List<TraitSO>();

    private FishController chaser;
    private float chasedTimer;

    public FishSO Data { get { return data; } }
    public float Size { get { return data.baseSize * sizeMult; } }
    public float CurrentSpeed { get { return data.fishSpeed * speedMult; } }

    public void Init(FishSO so, List<TraitSO> newTraits = null)
    {
        data = so;
        fullness = data.startFullness;
        if (sr != null) sr.sprite = data.sprite;

        traits = newTraits != null ? newTraits : new List<TraitSO>();
        sizeMult = 1f;
        speedMult = 1f;
        turnMult = 1f;
        hungerMult = 1f;

        foreach (TraitSO t in traits)
        {
            sizeMult *= t.sizeMultiplier;
            speedMult *= t.speedMultiplier;
            turnMult *= t.turnSpeedMultiplier;
            hungerMult *= t.hungerDecayMultiplier;
        }

        chaser = null;
        chasedTimer = 0f;

        transform.localScale = Vector3.one * Size;
        heading = Random.Range(0f, 360f);
        PickNewTarget();
    }

    public void NotifyChased(FishController predator)
    {
        chaser = predator;
        chasedTimer = fearMemoryTime;
    }

    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    private void Start()
    {
        if (data != null) Init(data, traits);
    }

    private void Update()
    {
        if (data == null) return;

        CalculateBounds();
        fullness = Mathf.Max(0f, fullness - data.fullnessDecayPerSecond * hungerMult * Time.deltaTime);

        if (chaser != null)
        {
            chasedTimer -= Time.deltaTime;
            if (chasedTimer <= 0f) chaser = null;
        }

        if (chaser != null) Flee();
        else if (fullness > data.wanderThreshold) Wander();
        else SeekFood();

        UpdateIcons();
    }

    private void LateUpdate()
    {
        if (iconRenderer == null || data == null || sr == null) return;

        Bounds b = sr.bounds;
        Transform t = iconRenderer.transform;
        t.rotation = Quaternion.identity;
        t.position = new Vector3(b.center.x, b.max.y + iconPadding, transform.position.z);
        t.localScale = Vector3.one * (iconSize / Size);
    }

    private void UpdateIcons()
    {
        if (iconRenderer == null) return;
        if (statusIcons == null)
        {
            Debug.LogWarning("FishController: statusIcons não atribuído em " + name);
            return;
        }

        bool afraid = chaser != null;
        bool hungry = fullness <= data.wanderThreshold;

        if (afraid) iconRenderer.sprite = statusIcons.fear;
        else if (hungry) iconRenderer.sprite = statusIcons.hunger;

        iconRenderer.enabled = afraid || hungry;
    }

    private void CalculateBounds()
    {
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector2 c = cam.transform.position;

        min = new Vector2(c.x - halfW + boundsMargin, c.y - halfH + boundsMargin);
        max = new Vector2(c.x + halfW - boundsMargin, c.y + halfH - boundsMargin);
    }

    private Vector2 ClampToBounds(Vector2 p)
    {
        return new Vector2(Mathf.Clamp(p.x, min.x, max.x), Mathf.Clamp(p.y, min.y, max.y));
    }

    private void PickNewTarget()
    {
        CalculateBounds();
        target = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        retargetTimer = Random.Range(data.minRetargetTime, data.maxRetargetTime);
    }

    private void Wander()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f || Vector2.Distance(transform.position, target) < 0.3f)
            PickNewTarget();

        MoveTo(target, CurrentSpeed);
    }

    private void Flee()
    {
        Vector2 pos = transform.position;
        Vector2 away = (pos - (Vector2)chaser.transform.position).normalized;

        Vector2 dest = ClampToBounds(pos + away * fleeDistance);

        if (Vector2.Distance(dest, pos) < 1f)
        {
            Vector2 center = (min + max) * 0.5f;
            Vector2 perp = new Vector2(-away.y, away.x);
            if (Vector2.Dot(perp, center - pos) < 0f) perp = -perp;
            dest = ClampToBounds(pos + perp * fleeDistance);
        }

        float speed = Mathf.Min(CurrentSpeed, chaser.CurrentSpeed * fleeSpeedRatio);
        MoveTo(dest, speed);
    }

    private void SeekFood()
    {
        Food food = null;
        FishController prey = null;

        if (data.diet != FishDiet.Carnivore) food = FindNearestFood();
        if (data.diet != FishDiet.Herbivore) prey = FindNearestPrey();

        if (food == null && prey == null)
        {
            Wander();
            return;
        }

        float dFood = food != null ? Vector2.Distance(transform.position, food.transform.position) : float.MaxValue;
        float dPrey = prey != null ? Vector2.Distance(transform.position, prey.transform.position) : float.MaxValue;
        float eatDist = data.eatDistance * Size;

        if (dFood <= dPrey)
        {
            MoveTo(food.transform.position, CurrentSpeed);

            if (dFood <= eatDist)
            {
                fullness = Mathf.Min(100f, fullness + food.FullnessRestore);
                Destroy(food.gameObject);
            }
        }
        else
        {
            if (dPrey <= prey.detectionRadius) prey.NotifyChased(this);
            MoveTo(prey.transform.position, CurrentSpeed);

            if (dPrey <= eatDist)
            {
                fullness = Mathf.Min(100f, fullness + prey.data.nutrition * prey.Size);
                Destroy(prey.gameObject);
            }
        }
    }

    private Food FindNearestFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        Food nearest = null;
        float best = float.MaxValue;

        foreach (GameObject f in foods)
        {
            Food food = f.GetComponent<Food>();
            if (food == null) continue;

            float d = Vector2.Distance(transform.position, f.transform.position);
            if (d < best)
            {
                best = d;
                nearest = food;
            }
        }
        return nearest;
    }

    private bool CanEat(FishController other)
    {
        if (other == null || other == this || other.data == null) return false;
        if (other.data.diet == FishDiet.Carnivore) return false;
        return other.Size <= Size;
    }

    private FishController FindNearestPrey()
    {
        FishController nearest = null;
        float best = float.MaxValue;

        foreach (FishController other in All)
        {
            if (!CanEat(other)) continue;

            float d = Vector2.Distance(transform.position, other.transform.position);
            if (d < best)
            {
                best = d;
                nearest = other;
            }
        }
        return nearest;
    }

    private void MoveTo(Vector2 dest, float speed)
    {
        Vector2 pos = transform.position;
        Vector2 toTarget = dest - pos;

        float desiredAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        float diff = Mathf.DeltaAngle(heading, desiredAngle);
        heading = Mathf.MoveTowardsAngle(heading, desiredAngle, turnSpeed * turnMult * Time.deltaTime);

        float speedFactor = Mathf.Lerp(0.4f, 1f, Mathf.Clamp01(Mathf.Cos(diff * Mathf.Deg2Rad) * 0.5f + 0.5f));
        float headingRad = heading * Mathf.Deg2Rad;
        Vector2 forward = new Vector2(Mathf.Cos(headingRad), Mathf.Sin(headingRad));

        Vector2 next = pos + forward * speed * speedFactor * Time.deltaTime;
        next = ClampToBounds(next);
        transform.position = new Vector3(next.x, next.y, transform.position.z);

        UpdateVisuals(forward);
    }

    private void UpdateVisuals(Vector2 forward)
    {
        float offset = spriteFacesLeft ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0f, 0f, heading - offset);

        if (sr != null)
            sr.flipY = spriteFacesLeft ? forward.x > 0f : forward.x < 0f;
    }
}