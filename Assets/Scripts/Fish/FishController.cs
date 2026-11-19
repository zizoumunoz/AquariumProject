using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

using static StatsEnum;

public class FishController : SerializedMonoBehaviour, IFishBehaviour
{
    public FishSO data;
    public FishStats stats;

    [SerializeField] public Dictionary<StatsEnum, float> multiplier;
    public List<FishAction> actions;
    public FishAction activeAction;
    [Header("Direction")]
    private bool spriteFacesLeft = true;
    [SerializeField] private float turnSpeed = 120f;
    private float breedTimer;
    [Header("Growth")]
    private Age age = Age.Young;
    public float growTimer;
    private Camera cam;
    [SerializeField] private SpriteRenderer sr;
    public SpriteRenderer highlight;
    private Vector2 min, max;
    private Vector2 target;
    private float retargetTimer;
    [SerializeField] private float fullness = 0;
    private float heading;

    private FishController chaser;
    private float chasedTimer;
    public int Price { get { return Mathf.RoundToInt(data.price * priceMultiplier * (age == Age.Adult ? 2 : 0.5f)); } }
    private float priceMultiplier = 1;
    
    [SerializeField] private float happiness = 0;

    // used for the animation! :P
    private float normalizedSpeed;
    public float NormalizedSpeed { get { return normalizedSpeed; } }

    // used for the poop!
    public FoodSource aimedFoodSorce = null;
    private float eatTimer = 5f;

    public void Init(FishStats stats)
    {
        this.stats = stats;
        data = stats.baseFish;

        this.growTimer = growTimer != -1f ? growTimer : stats[ageTimer];
        sr.sprite = data.sprite;
        highlight.sprite = data.sprite;


        chaser = null;
        chasedTimer = 0f;

        transform.localScale = Vector3.one * stats[baseSize];
        heading = UnityEngine.Random.Range(0f, 360f);
        PickNewTarget();
    }
    public void SetAction(string newActionName)
    {
        activeAction?.Exit();
        activeAction = actions.Find(x => x.actionName == newActionName);
        activeAction.Enter();
    }

    private void OnDestroy()
    {
        if (aimedFoodSorce != null) aimedFoodSorce.RemoveTarget(this);
    }

    public void UpdateFish()
    {
        fullness = Mathf.Max(0f, fullness - stats[fullnessDecayPerSecond] * Time.deltaTime); breedTimer = Mathf.Max(0f, breedTimer - Time.deltaTime);

        if (chaser != null)
        {
            chasedTimer -= Time.deltaTime;
            if (chasedTimer <= 0f) chaser = null;
        }
        else if (fullness <= stats[wanderThreshold] && eatTimer < 0)
        {
            SeekFood(); 
        }
        else if (!TryBreed()) Wander();

        eatTimer -= Time.deltaTime;
        UpdateHappiness();
        UpdateGrowth();
    }



    private void UpdateGrowth()
    {
        if (age != Age.Young) return;

        growTimer -= Time.deltaTime;
        if (growTimer > 0f) return;

        age = Age.Adult;
        transform.localScale = Vector3.one * stats[baseSize];
        GetComponent<FoodSource>().size = stats[baseSize];
    }

    private void UpdateHappiness()
    {
        if (fullness < stats[unhappyBelow])
            happiness -= stats[happinessLossPerSecond] * Time.deltaTime;
        else if (fullness > stats[happyAbove])
            happiness += stats[happinessGainPerSecond] * Time.deltaTime;

        happiness = Mathf.Clamp(happiness, 0f, 100f);
    }

    private void LateUpdate()
    {
        if (data == null || data.iconRenderer == null || sr == null) return;

        Bounds b = sr.bounds;
        Transform t = data.iconRenderer.transform;
        t.rotation = Quaternion.identity;
        t.position = new Vector3(b.center.x, b.max.y + data.iconPadding, transform.position.z);
        t.localScale = Vector3.one * (data.iconSize / stats[baseSize]);
    }

    private float BoundsMargin
    {
        get
        {
            Vector3 extents = sr.sprite.bounds.extents;
            return Mathf.Max(extents.x, extents.y) * stats[baseSize];
        }
    }
    private void Awake()
    {
        cam = Camera.main;
    }

    private void CalculateBounds()
    {
        float margin = BoundsMargin;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector2 c = cam.transform.position;

        min = new Vector2(c.x - halfW + margin, c.y - halfH + margin);
        max = new Vector2(c.x + halfW - margin, c.y + halfH - margin);
    }

    private Vector2 ClampToBounds(Vector2 p)
    {
        return new Vector2(Mathf.Clamp(p.x, min.x, max.x), Mathf.Clamp(p.y, min.y, max.y));
    }

    private void PickNewTarget()
    {
        CalculateBounds();
        target = new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        retargetTimer = UnityEngine.Random.Range(stats[minRetargetTime], stats[maxRetargetTime]);
    }

    private void Wander()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f || Vector2.Distance(transform.position, target) < 0.3f)
            PickNewTarget();

        MoveTo(target, stats[fishSpeed]);
    }

    private void SeekFood()
    {
        if (aimedFoodSorce != null && aimedFoodSorce.bitesLeft <= 0)
        {
            aimedFoodSorce.RemoveTarget(this);
            aimedFoodSorce = null;
        }

        if (aimedFoodSorce == null)
        {
            aimedFoodSorce = FindNearestFood();
            if (aimedFoodSorce == null)
            {
                Wander();
                return;
            }
            aimedFoodSorce.AddTarget(this);
        }

        MoveTo(aimedFoodSorce.transform.position, stats[fishSpeed]);

        float dFood = aimedFoodSorce.DistanceTo(transform.position);
        float eatDist = stats[baseSize];

        if (dFood <= eatDist)
        {
            aimedFoodSorce.RemoveTarget(this);
            fullness = Mathf.Min(100f, fullness + aimedFoodSorce.Eat());
            aimedFoodSorce = null;
            eatTimer = 1f;
        }
    }

    private FoodSource FindNearestFood()
    {
        FoodSource nearest = null;
        float best = float.MaxValue;

        Collider2D[] foods = Physics2D.OverlapCircleAll(transform.position, 50f, LayerMask.GetMask("Food", "Fish"));

        foreach (Collider2D food in foods)
        {
            FoodSource source = food.GetComponent<FoodSource>();
            bool available = source.IsAvailable();
            bool canEat = CanEatFood(source);
            // Debug.Log(name + " -> " + source.name + " | bites=" + source.bitesLeft + " | available=" + available + " | canEat=" + canEat, source);
            if (!available || !canEat) continue;
            if (source.CompareTag("Fish") && source.GetComponent<FishController>().data == data) continue;

            float d = Vector2.SqrMagnitude(transform.position - food.transform.position);
            if (d < best)
            {
                best = d;
                nearest = source;
            }
        }
        return nearest;
    }

    private bool CanEatFood(FoodSource food)
    {
        switch (data.diet)
        {
            case FishDiet.Herbivore: return food.type == FoodType.Plant;
            case FishDiet.Carnivore:
                {
                    return food.type == FoodType.Meat && stats[baseSize] >= food.size;
                }
            default: return true;
        }
    }

    private void MoveTo(Vector2 dest, float speed)
    {
        Vector2 pos = transform.position;
        Vector2 toTarget = dest - pos;

        float desiredAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        float diff = Mathf.DeltaAngle(heading, desiredAngle);
        heading = Mathf.MoveTowardsAngle(heading, desiredAngle, turnSpeed * Time.deltaTime);

        float speedFactor = Mathf.Lerp(0.4f, 1f, Mathf.Clamp01(Mathf.Cos(diff * Mathf.Deg2Rad) * 0.5f + 0.5f));
        normalizedSpeed = stats[fishSpeed] > 0f ? Mathf.Clamp01(speed * speedFactor / stats[fishSpeed]) : 0f;
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

        sr.flipY = spriteFacesLeft ? forward.x > 0f : forward.x < 0f;
        highlight.flipY = spriteFacesLeft ? forward.x > 0f : forward.x < 0f;
    }

    private bool CanBreed
    {
        get
        {
            return age == Age.Adult
                && happiness >= stats[happyAbove]
                && breedTimer <= 0f
                && chaser == null
                && fullness > stats[wanderThreshold];
        }
    }
    private bool TryBreed()
    {
        if (!CanBreed) return false;

        FishController mate = FindMate();
        if (mate == null) return false;

        MoveTo(mate.transform.position, stats[fishSpeed]);

        return true;
    }

    private FishController FindMate()
    {
        FishController nearest = null;
        float best = float.MaxValue;

        Collider2D[] fishes = Physics2D.OverlapCircleAll(transform.position, 50f, LayerMask.GetMask("Fish"));
        foreach (Collider2D otherCollider in fishes)
        {
            FishController other = otherCollider.GetComponent<FishController>();
            if (other == this || other.data != data || !other.CanBreed) continue;

            float d = Vector2.Distance(transform.position, other.transform.position);
            if (d < best)
            {
                best = d;
                nearest = other;
            }
        }
        return nearest;
    }
}

[Serializable]
public class FishAction
{
    protected FishController controller;
    protected FishStats stats;
    protected FishSO so;
    public string actionName;
    public FishAction(FishController controller, FishStats stats, FishSO so, string actionName)
    {
        this.controller = controller;
        this.stats = stats;
        this.so = so;
        this.actionName = actionName;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
[Serializable]
public class WanderAction : FishAction
{
    public WanderAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
    }

    public override void Enter(){}

    public override void Exit(){}

    public override void Update(){}
}
[Serializable]
public class FleeAction : FishAction
{
    protected FoodSource source;
    public FleeAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
    }

    public override void Enter()
    {
        stats.statsMultiplier[fishSpeed] *= stats.statsMultiplier[fleeSpeedMultiplier];
    }

    public override void Exit()
    {
        stats.statsMultiplier[fishSpeed] /= stats.statsMultiplier[fleeSpeedMultiplier];
    }

    public override void Update()
    {
        if (!source.IsTargeted())
        {
            controller.SetAction("Wander");
        }
    }
}
[Serializable]
public class MateAction : FishAction
{
    public MateAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
    }

    public override void Enter(){}

    public override void Exit(){}

    public override void Update(){}
}
[Serializable]
public class SeekFoodAction : FishAction
{
    public SeekFoodAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
    }

    public override void Enter(){}

    public override void Exit(){}

    public override void Update(){}
}
