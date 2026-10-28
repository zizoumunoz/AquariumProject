using System.Reflection;
using UnityEngine;

/// <summary>
/// Makes a fish eat poop. FishRole adds and enables this when the role is set to Cleaner.
/// When the fish is hungry it steers toward the nearest poop and eats it.
///
/// It works WITHOUT editing FishController by reading and setting a few of its
/// private fields: fullness, target, retargetTimer and chaser.
/// If someone renames those fields, this script logs an error and stops working.
/// </summary>
[RequireComponent(typeof(FishController))]
[DefaultExecutionOrder(-50)] // runs before FishController, so our target is used this frame
public class PoopEater : MonoBehaviour
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

    [Tooltip("The fish looks for poop when its fullness is at or below this (0 to 100).")]
    [SerializeField] private float hungryBelow = 60f;
    [Tooltip("How close the fish must get to eat the poop (scaled by the fish size).")]
    [SerializeField] private float eatDistance = 0.4f;

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
            Debug.LogError("PoopEater: FishController no longer has the private fields " +
                           "'fullness', 'target' and 'retargetTimer'. Someone renamed them. " +
                           "Update the names in PoopEater.cs.", this);
        }
    }

    private void Update()
    {
        if (!ready || controller.Data == null) return;

        // Only go for poop when hungry
        float fullness = (float)fullnessField.GetValue(controller);
        if (fullness > hungryBelow) return;

        // Running away from a predator comes first
        if (chaserField != null)
        {
            FishController chaser = chaserField.GetValue(controller) as FishController;
            if (chaser != null) return;
        }

        Poop poop = Poop.FindNearest(transform.position);
        if (poop == null) return;

        float distance = Vector2.Distance(transform.position, poop.transform.position);
        float reach = Mathf.Max(eatDistance * controller.Size, 0.35f);

        if (distance <= reach)
        {
            fullnessField.SetValue(controller, Mathf.Min(100f, fullness + poop.FullnessRestore));
            Destroy(poop.gameObject);
            return;
        }

        // Point the fish's normal wandering at the poop, and keep it from picking a new target
        targetField.SetValue(controller, (Vector2)poop.transform.position);
        retargetTimerField.SetValue(controller, 1f);
    }
}