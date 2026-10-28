using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Makes a fish poop a few seconds after it eats.
/// FishPoopSetup adds this to every fish automatically, so you don't need to add it by hand.
/// Fish with PoopEater (the cleaners) never poop, otherwise cleaning would be pointless.
///
/// It notices a meal by watching the private 'fullness' field of FishController jump up.
/// If someone renames that field, this script logs an error and turns itself off.
/// </summary>
[RequireComponent(typeof(FishController))]
[DefaultExecutionOrder(50)] // runs after FishController, so it sees this frame's eating
public class FishPooper : MonoBehaviour
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

    [Tooltip("Seconds between eating and pooping (random between these two).")]
    [SerializeField] private float minDigestTime = 2f;
    [SerializeField] private float maxDigestTime = 5f;
    [Tooltip("How much fullness has to jump at once to count as a meal.")]
    [SerializeField] private float minMealGain = 1f;

    private FishController controller;
    private FieldInfo fullnessField;
    private float lastFullness = -1f;
    private readonly List<float> digestTimers = new List<float>();

    private void Awake()
    {
        controller = GetComponent<FishController>();
        fullnessField = typeof(FishController).GetField("fullness", Flags);

        if (fullnessField == null)
        {
            Debug.LogError("FishPooper: FishController no longer has the private field " +
                           "'fullness'. Someone renamed it. Update the name in FishPooper.cs.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (controller.Data == null) return;

        float fullness = (float)fullnessField.GetValue(controller);

        // Fullness only goes up when the fish eats (it slowly goes down otherwise)
        bool ate = lastFullness >= 0f && fullness - lastFullness >= minMealGain;
        if (ate && GetComponent<PoopEater>() == null)
            digestTimers.Add(Random.Range(minDigestTime, maxDigestTime));

        lastFullness = fullness;

        for (int i = digestTimers.Count - 1; i >= 0; i--)
        {
            digestTimers[i] -= Time.deltaTime;
            if (digestTimers[i] > 0f) continue;

            Poop.Spawn(transform.position);
            digestTimers.RemoveAt(i);
        }
    }
}