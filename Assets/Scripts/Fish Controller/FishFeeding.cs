using UnityEngine;

[RequireComponent(typeof(FishController), typeof(FishMovement))]
public class FishFeeding : MonoBehaviour, IFishBehaviour
{
    private const float InititalEatDelay = 5.0f;
    private const float EatColldown = 1.0f;
    private const float SearchRadius = 50.0f;

    private FishController fish;
    private FishMovement movement;

    public FoodSource aimedFood;
    private float eatTimer = InititalEatDelay;

    private void Awake()
    {
        fish = GetComponent<FishController>();
        movement = GetComponent<FishMovement>();
    }

    private void OnDestroy()
    {
        ReleaseTarget();
    }

    private void Update()
    {
        eatTimer -= Time.deltaTime;
    }

    public bool Tick()
    {
        if (!fish.IsHungry)
        {
            ReleaseTarget();
            return false;
        }

        if (eatTimer >= 0.0f) return false;

        if (aimedFood != null && aimedFood.bitesLeft <= 0)
        {
            ReleaseTarget();
        }

        if (aimedFood == null && !AquireTarget()) return false;

        movement.MoveTo(aimedFood.transform.position, fish.CurrentSpeed);
        TryEat();
        return true;
    }

    private void ReleaseTarget()
    {
        if (aimedFood != null) aimedFood.RemoveTarget(fish);
        aimedFood = null;
    }

    private bool AquireTarget()
    {
        FoodSource nearest = FindNearestFood();
        if (nearest == null) return false;

        aimedFood = nearest;
        aimedFood.AddTarget(fish);
        return true;

    }

    private void TryEat()
    {
        float distance = aimedFood.DistanceTo(movement.Body);
        float eatDistance = fish.Data.fishStatus.eatDistance * fish.statusMultiplier.eatDistance * fish.Size;

        if (distance > eatDistance) return;

        aimedFood.RemoveTarget(fish);
        fish.Feed(aimedFood.Eat());
        aimedFood = null;
        eatTimer = EatColldown;
    }

    private FoodSource FindNearestFood()
    {
        FoodSource nearest = null;
        float best = float.MaxValue;

        Collider2D[] foods = Physics2D.OverlapCircleAll(transform.position, SearchRadius, LayerMask.GetMask("Food", "Fish"));

        foreach (Collider2D food in foods)
        {
            FoodSource source = food.GetComponent<FoodSource>();
            if (!source.IsAvailable() || !CanEat(source)) continue;
            if (source.CompareTag("Fish") && source.GetComponent<FishController>().Data == fish.Data) continue;

            float d = Vector2.SqrMagnitude(transform.position - food.transform.position);
            if (d < best)
            {
                best = d;
                nearest = source;
            }
        }
        return nearest;
    }

    private bool CanEat(FoodSource food)
    {
        switch (fish.data.diet)
        {
            case FishDiet.Herbivore: return food.type == FoodType.Plant;
            case FishDiet.Carnivore: return food.type == FoodType.Meat && fish.Size >= food.size;
            case FishDiet.AlgaeEater: return food.type == FoodType.Algae;
            default: return food.type != FoodType.Algae;
        }
    }



}
