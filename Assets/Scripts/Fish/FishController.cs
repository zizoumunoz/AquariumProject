using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static StatsEnum;

public class FishController : SerializedMonoBehaviour, IFishBehaviour
{
    public FishSO data;
    public FishStats stats;
    public List<FishAction> actions;
    [SerializeField] public FishAction activeAction;

    public int Order => 3;

    public void Init(FishStats stats)
    {
        this.stats = stats;
        data = stats.baseFish;

        actions = new()
        {
            new WanderAction(this, stats, data, "Wander"),
            new SeekFoodAction(this, stats, data, "SeekFood")
        };

        SetAction("Wander");
    }
    public void SetAction(string newActionName)
    {
        Debug.Log(activeAction.actionName + " -> " + newActionName);
        activeAction?.Exit();
        activeAction = actions.Find(x => x.actionName == newActionName);
        activeAction.Enter();
    }

    public void UpdateFish()
    {
        activeAction.Update();
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
    public Direction direction;
    protected FishBounds bounds;
    protected FishMovement movement;
    public WanderAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
        bounds = stats.GetBehaviour<FishBounds>();
        movement = stats.GetBehaviour<FishMovement>();
    }

    public override void Enter() { }

    public override void Exit() { }

    public override void Update()
    {
        if (stats.fullness < stats[wanderThreshold])
        {
            foreach (Collider2D col in bounds.fishFoodRadius)
            {
                if (DietFoodConversion.CanEat(stats, col.GetComponent<FoodSource>()))
                {
                    controller.SetAction("SeekFood");
                    return;
                }
            }
        }
        if (bounds.distanceToWalls[Direction.front] < stats[baseSize] + 0.02f * stats[turnSpeed] / stats[fishSpeed])
        {
            if (direction == Direction.up)
            {
                movement.TurnUp();
            }
            else if (direction == Direction.down)
            {
                movement.TurnDown();
            }
            if (bounds.distanceToWalls[Direction.downFront] > bounds.distanceToWalls[Direction.upFront])
            {
                direction = Direction.up;
                movement.TurnUp();
            }
            else
            {
                direction = Direction.down;
                movement.TurnDown();
            }
        }
        else if (bounds.distanceToWalls[Direction.down] < stats[baseSize] + 0.01f * stats[turnSpeed] / stats[fishSpeed])
        {
            movement.TurnDown();
        }
        else if (bounds.distanceToWalls[Direction.up] < stats[baseSize] + 0.01f * stats[turnSpeed] / stats[fishSpeed])
        {
            movement.TurnUp();
        }
        else
        {
            direction = Direction.back;
        }
    }
}
[Serializable]
public class FleeAction : FishAction
{
    protected FoodSource source;
    public FleeAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
        stats.GetComponent<FoodSource>();
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

    public override void Enter() { }

    public override void Exit() { }

    public override void Update() { }
}
[Serializable]
public class SeekFoodAction : FishAction
{
    protected FishMovement movement;
    protected FishBounds bounds;
    public float retargetTimer = 0;
    public SeekFoodAction(FishController controller, FishStats stats, FishSO so, string actionName) : base(controller, stats, so, actionName)
    {
        movement = stats.GetBehaviour<FishMovement>();
        bounds = stats.GetBehaviour<FishBounds>();
    }

    public override void Enter()
    {
        foreach (Collider2D col in bounds.fishFoodRadius)
        {
            if (DietFoodConversion.CanEat(stats, col.GetComponent<FoodSource>()))
            {
                movement.SetTarget(col.transform);
                col.GetComponent<FoodSource>().AddTarget(controller);
                break;
            }
        }
        retargetTimer = UnityEngine.Random.Range(stats[minRetargetTime], stats[maxRetargetTime]);
    }

    public override void Exit()
    {
        movement.SetTarget(null);
    }

    public override void Update()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer < 0)
        {
            if (movement.target != null)
            {
                movement.target.GetComponent<FoodSource>().RemoveTarget(controller);
            }
            controller.SetAction("Wander");
        }
        foreach (Collider2D col in bounds.mouthCollisions)
        {
            if (col.transform == movement.target)
            {
                movement.target.GetComponent<FoodSource>().RemoveTarget(controller);
                stats.fullness += movement.target.GetComponent<FoodSource>().Eaten();
                controller.SetAction("Wander");
                break;
            }
        }
    }
}
