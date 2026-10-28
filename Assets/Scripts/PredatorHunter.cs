using System.Reflection;
using UnityEngine;

/// <summary>
/// Makes a fish hunt Cleaner fish (fish whose FishRole is Cleaner).
/// FishRole adds and enables this when the role is set to Predator.
/// When hungry it steers toward the nearest cleaner, scares it (so it flees), and eats it.
///
/// Like PoopEater, it works WITHOUT editing FishController by using a few of its
/// private fields: fullness, target, retargetTimer and chaser.
/// If someone renames those fields, this script logs an error and stops working.
/// </summary>
[RequireComponent(typeof(FishController))]
[DefaultExecutionOrder(-50)] // runs before FishController, so our target is used this frame
public class PredatorHunter : MonoBehaviour
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

    [Tooltip("The fish hunts when its fullness is at or below this (0 to 100).")]
    [SerializeField] private float hungryBelow = 60f;
    [Tooltip("How close the fish must get to eat a cleaner (scaled by the fish size).")]
    [SerializeField] private float eatDistance = 0.6f;
    [Tooltip("Cleaners closer than this notice the predator and flee.")]
    [SerializeField] private float scareRadius = 3f;
    [Tooltip("Fullness gained from eating a cleaner, multiplied by the cleaner's size.")]
    [SerializeField] private float fullnessPerPreySize = 30f;

    private FishController controller;
    private FieldInfo fullnessField;
    private FieldInfo targetField;
    private FieldInfo retargetTimerField;
    private FieldInfo chaserField;
    private bool ready;

    private void Awake()
    {
        controller = GetComponent<FishController>();

        System.Type type = typeof(FishController);
        fullnessField = type.GetField("fullness", Flags);
        targetField = type.GetField("target", Flags);
        retargetTimerField = type.GetField("retargetTimer", Flags);
        chaserField = type.GetField("chaser", Flags); // optional

        ready = fullnessField != null && targetField != null && retargetTimerField != null;
        if (!ready)
        {
            Debug.LogError("PredatorHunter: FishController no longer has the private fields " +
                           "'fullness', 'target' and 'retargetTimer'. Someone renamed them. " +
                           "Update the names in PredatorHunter.cs.", this);
        }
    }

    private void Update()
    {
        if (!ready || controller.Data == null) return;

        // Only hunt when hungry
        float fullness = (float)fullnessField.GetValue(controller);
        if (fullness > hungryBelow) return;

        // Running away from something bigger comes first
        if (chaserField != null)
        {
            FishController chaser = chaserField.GetValue(controller) as FishController;
            if (chaser != null) return;
        }

        FishController prey = FindNearestCleaner();
        if (prey == null) return;

        float distance = Vector2.Distance(transform.position, prey.transform.position);
        float reach = Mathf.Max(eatDistance * controller.Size, 0.5f);

        if (distance <= scareRadius)
            prey.NotifyChased(controller); // makes the cleaner flee

        if (distance <= reach)
        {
            float gained = fullnessPerPreySize * prey.Size;
            fullnessField.SetValue(controller, Mathf.Min(100f, fullness + gained));
            Destroy(prey.gameObject);
            return;
        }

        // Point the fish's normal wandering at the cleaner
        targetField.SetValue(controller, (Vector2)prey.transform.position);
        retargetTimerField.SetValue(controller, 1f);
    }

    private FishController FindNearestCleaner()
    {
        FishController nearest = null;
        float best = float.MaxValue;

        foreach (FishRole other in FishRole.All)
        {
            if (other.Role != FishRoleType.Cleaner) continue;
            if (other.Controller == null || other.Controller == controller) continue;

            float d = Vector2.Distance(transform.position, other.transform.position);
            if (d < best)
            {
                best = d;
                nearest = other.Controller;
            }
        }
        return nearest;
    }
}