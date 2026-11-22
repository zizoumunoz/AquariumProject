using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(FishController), typeof(FishMovement))]
public class FishBreeding : MonoBehaviour, IFishBehaviour
{
    private const float searchRadius = 50f;

    private FishController fish;
    private FishMovement movement;
    private float cooldown;

    private void Awake()
    {
        fish = GetComponent<FishController>();
        movement = GetComponent<FishMovement>();
    }

    private void Update()
    {
        cooldown = Mathf.Max(0.0f, cooldown - Time.deltaTime);
    }

    public bool CanBreed
    {
        get
        {
            return fish.CurrentAge == Age.Adult
                && fish.Happiness >= fish.Data.fishStatus.breedHappiness * fish.statusMultiplier.breedHappiness
                && cooldown <= 0f
                && !fish.IsAfraid
                && !fish.IsHungry;
        }
    }

    public bool Tick()
    {
        if (!CanBreed) return false;

        FishBreeding mate = FindMate();
        if (mate == null) return false;

        movement.MoveTo(mate.transform.position, fish.CurrentSpeed);

        if (DistanceTo(mate) <= BreedDistance) Breed(mate);

        return true;
    }

    private float BreedDistance
    {
        get { return fish.Data.fishStatus.breedDistance * fish.statusMultiplier.breedDistance; }
    }

    private float DistanceTo(FishBreeding other)
    {
        return Mathf.Max(0.0f, movement.Body.Distance(other.movement.Body).distance);
    }

    private FishBreeding FindMate()
    {
        FishBreeding nearest = null;
        float best = float.MaxValue;

        Collider2D[] fishes = Physics2D.OverlapCircleAll(transform.position, searchRadius, LayerMask.GetMask("Fish"));

        foreach (Collider2D other in fishes)
        {
            FishBreeding candidate = other.GetComponent<FishBreeding>();
            if (candidate == null || candidate == this) continue;
            if (candidate.fish.Data != fish.Data || !candidate.CanBreed) continue;

            float d = Vector2.Distance(transform.position, candidate.transform.position);
            if (d < best)
            {
                best = d;
                nearest = candidate;
            }
        }
        return nearest;
    }

    private void Breed(FishBreeding mate)
    {
        Vector2 pos = (transform.position + mate.transform.position) * 0.5f;
        if (!FishSpawner.instance.SpawnChild(fish.Data, fish.traits, mate.fish.traits, pos)) return;

        StartCooldown();
        mate.StartCooldown();
    }

    private void StartCooldown()
    {
        cooldown = fish.Data.fishStatus.breedCooldown * fish.statusMultiplier.breedCooldown;
    }

}
