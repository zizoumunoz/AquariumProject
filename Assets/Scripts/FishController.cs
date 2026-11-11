using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour, IMotionSource
{
    public FishSO data;

    public FishStatus statusMultiplier = new();

    private const float boundsMargin = 0.5f;

    [Header("Direction")]
    private bool spriteFacesLeft = true;
    [SerializeField] private float turnSpeed = 120f;

    private float breedTimer;
    private bool initialized;

    [Header("Growth")]

    private Age age = Age.Young;
    public float growTimer;


    private Camera cam;
    [SerializeField] private SpriteRenderer sr;
    private Vector2 min, max;
    private Vector2 target;
    private float retargetTimer;
    [SerializeField] private float fullness;
    private float heading;
    public List<TraitSO> traits = new List<TraitSO>();

    private FishController chaser;
    private float chasedTimer;

    public FishSO Data { get { return data; } }
    public float Size { get { return data.fishStatus.baseSize * statusMultiplier.baseSize * AgeMultiplier; } }
    private float AgeMultiplier { get { return age == Age.Young ? data.fishStatus.youngSizeMultiplier * statusMultiplier.youngSizeMultiplier : 1f; } }
    public float CurrentSpeed { get { return data.fishStatus.fishSpeed * statusMultiplier.fishSpeed; } }

    private float happiness;
    public float Happiness { get { return happiness; } }
    public Age CurrentAge { get { return age; } }

    // used for the animation! :P
    private float normalizedSpeed;
    public float NormalizedSpeed { get { return normalizedSpeed; } }

    // used for the poop!
    public bool IsHungry { get { return fullness <= data.fishStatus.wanderThreshold * statusMultiplier.wanderThreshold; } }
    public float PoopInterval { get { return data.fishStatus.poopInterval * statusMultiplier.poopInterval; } }
    public FoodSource aimedFoodSorce = null;
    private float eatTimer = 5f;

    public void Init(FishSO so, FishStatus status, List<TraitSO> newTraits = null, float growTimer = -1f)
    {
        data = so;
        FishCollection.Discover(so);    // Calls FishCollection event to add to album
        statusMultiplier = status;
        fullness = data.fishStatus.startFullness * statusMultiplier.startFullness;
        happiness = data.fishStatus.happiness * statusMultiplier.happiness;
        initialized = true;

        this.growTimer = growTimer != -1f ? growTimer : data.fishStatus.ageTime * statusMultiplier.ageTime;
        if (sr != null) sr.sprite = data.sprite;

        traits = newTraits ?? new List<TraitSO>();

        chaser = null;
        chasedTimer = 0f;

        transform.localScale = Vector3.one * Size;
        heading = Random.Range(0f, 360f);
        PickNewTarget();
    }

    public void NotifyChased(FishController predator)
    {
        chaser = predator;
        chasedTimer = data.fishStatus.fearMemoryTime * statusMultiplier.fearMemoryTime;
    }

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        if (data != null && !initialized) Init(data, statusMultiplier, traits);
    }

    private void Update()
    {
        CalculateBounds();
        fullness = Mathf.Max(0f, fullness - data.fishStatus.fullnessDecayPerSecond * statusMultiplier.fullnessDecayPerSecond * Time.deltaTime);
        breedTimer = Mathf.Max(0f, breedTimer - Time.deltaTime);

        if (chaser != null)
        {
            chasedTimer -= Time.deltaTime;
            if (chasedTimer <= 0f) chaser = null;
        }

        if (chaser != null) Flee();
        else if (fullness <= data.fishStatus.wanderThreshold * statusMultiplier.wanderThreshold && eatTimer < 0)
        {
            SeekFood(); 
        }
        else if (!TryBreed()) Wander();

        eatTimer -= Time.deltaTime;
        UpdateHappiness();
        UpdateGrowth();
        UpdateIcons();
    }



    private void UpdateGrowth()
    {
        if (age != Age.Young) return;

        growTimer -= Time.deltaTime;
        if (growTimer > 0f) return;

        age = Age.Adult;
        transform.localScale = Vector3.one * Size;
        GetComponent<FoodSource>().size = Size;
    }

    private void UpdateHappiness()
    {
        if (fullness < data.fishStatus.unhappyBelow * statusMultiplier.unhappyBelow)
            happiness -= data.fishStatus.happinessLossPerSecond * statusMultiplier.happinessLossPerSecond * Time.deltaTime;
        else if (fullness > data.fishStatus.happyAbove * statusMultiplier.happyAbove)
            happiness += data.fishStatus.happinessGainPerSecond * statusMultiplier.happinessGainPerSecond * Time.deltaTime;

        happiness = Mathf.Clamp(happiness, 0f, 100f);
    }

    private void LateUpdate()
    {
        if (data == null || data.iconRenderer == null || sr == null) return;

        Bounds b = sr.bounds;
        Transform t = data.iconRenderer.transform;
        t.rotation = Quaternion.identity;
        t.position = new Vector3(b.center.x, b.max.y + data.iconPadding, transform.position.z);
        t.localScale = Vector3.one * (data.iconSize / Size);
    }

    private void UpdateIcons()
    {
        if (data.iconRenderer == null) return;
        if (data.statusIcons == null)
        {
            Debug.LogWarning("missing " + name);
            return;
        }

        bool afraid = chaser != null;
        bool hungry = fullness <= data.fishStatus.wanderThreshold * statusMultiplier.wanderThreshold;

        if (afraid) data.iconRenderer.sprite = data.statusIcons.fear;
        else if (hungry) data.iconRenderer.sprite = data.statusIcons.hunger;

        data.iconRenderer.enabled = afraid || hungry;
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
        retargetTimer = Random.Range(data.fishStatus.minRetargetTime * statusMultiplier.minRetargetTime, data.fishStatus.maxRetargetTime * statusMultiplier.maxRetargetTime);
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

        Vector2 dest = ClampToBounds(pos + away * data.fishStatus.fleeDistance * statusMultiplier.fleeDistance);

        if (Vector2.Distance(dest, pos) < 1f)
        {
            Vector2 center = (min + max) * 0.5f;
            Vector2 perp = new Vector2(-away.y, away.x);
            if (Vector2.Dot(perp, center - pos) < 0f) perp = -perp;
            dest = ClampToBounds(pos + perp * data.fishStatus.fleeDistance * statusMultiplier.fleeDistance);
        }

        float speed = Mathf.Min(CurrentSpeed, chaser.CurrentSpeed * data.fishStatus.fleeSpeedRatio * statusMultiplier.fleeSpeedRatio);
        MoveTo(dest, speed);
    }

    private void SeekFood()
    {
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

        MoveTo(aimedFoodSorce.transform.position, CurrentSpeed);

        float dFood = Vector2.Distance(transform.position, aimedFoodSorce.transform.position);
        float eatDist = data.fishStatus.eatDistance * statusMultiplier.eatDistance * Size;

        if (dFood <= eatDist)
        {
            aimedFoodSorce.RemoveTarget(this);
            fullness = Mathf.Min(100f, fullness + aimedFoodSorce.Eat());
            aimedFoodSorce = null;
            eatTimer = 5f;
        }
    }

    private FoodSource FindNearestFood()
    {
        FoodSource nearest = null;
        float best = float.MaxValue;

        Collider2D[] foods = Physics2D.OverlapCircleAll(transform.position, 50f, LayerMask.GetMask("Food", "Fish"));

        foreach (Collider2D food in foods)
        {
            Debug.Log(food.gameObject.name);
            FoodSource source = food.GetComponent<FoodSource>();
            if (!source.IsAvailable() || !CanEatFood(source)) continue;
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
                    return food.type == FoodType.Meat && Size >= food.size;
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
        normalizedSpeed = CurrentSpeed > 0f ? Mathf.Clamp01(speed * speedFactor / CurrentSpeed) : 0f;
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

    private bool CanBreed
    {
        get
        {
            return age == Age.Adult
                && happiness >= data.fishStatus.breedHappiness * statusMultiplier.breedHappiness
                && breedTimer <= 0f
                && chaser == null
                && fullness > data.fishStatus.wanderThreshold * statusMultiplier.wanderThreshold;
        }
    }

    public void SetAge(Age newAge)
    {
        age = newAge;
        growTimer = data.fishStatus.ageTime * statusMultiplier.ageTime;
        transform.localScale = Vector3.one * Size;
    }
    public void LoadFish(Age newAge, float growTimer)
    {
        age = newAge;
        this.growTimer = growTimer;
        transform.localScale = Vector3.one * Size;
    }

    private bool TryBreed()
    {
        if (!CanBreed) return false;

        FishController mate = FindMate();
        if (mate == null) return false;

        MoveTo(mate.transform.position, CurrentSpeed);

        if (Vector2.Distance(transform.position, mate.transform.position) <= statusMultiplier.breedHappiness)
            Breed(mate);

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

    private void Breed(FishController mate)
    {
        List<TraitSO> childTraits = InheritTraits(mate);
        Vector3 pos = (transform.position + mate.transform.position) * 0.5f;

        FishSpawner.instance.SpawnChild(data,
            new FishStatus((float x, float y) => Random.Range(Mathf.Min(x, y) - 0.2f, Mathf.Max(x, y) + 0.2f),
            statusMultiplier, mate.statusMultiplier),
            childTraits, pos);

        breedTimer = data.fishStatus.breedCooldown * statusMultiplier.breedCooldown;
        mate.breedTimer = data.fishStatus.breedCooldown * mate.statusMultiplier.breedCooldown;
    }

    private List<TraitSO> InheritTraits(FishController mate)
    {
        List<TraitSO> pool = new List<TraitSO>();
        foreach (TraitSO t in traits) if (!pool.Contains(t)) pool.Add(t);
        foreach (TraitSO t in mate.traits) if (!pool.Contains(t)) pool.Add(t);

        List<TraitSO> result = new List<TraitSO>();
        foreach (TraitSO t in pool)
            if (Random.value < 0.5f) result.Add(t);

        if (result.Count == 0 && pool.Count > 0)
            result.Add(pool[Random.Range(0, pool.Count)]);

        return result;
    }
}