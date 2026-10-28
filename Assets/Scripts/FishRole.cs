using System.Collections.Generic;
using UnityEngine;

public enum FishRoleType
{
    Pooper,    // normal fish: eats, then poops
    Cleaner,   // eats poop, never poops
    Predator   // hunts the cleaner fish (and poops after eating)
}

/// <summary>
/// Add this to a fish prefab and pick its role in the Inspector.
/// The role turns the right behaviors on and off, so you never add
/// FishPooper, PoopEater or PredatorHunter by hand.
/// Fish without this component are treated as Poopers (FishPoopSetup adds it).
/// </summary>
[RequireComponent(typeof(FishController))]
public class FishRole : MonoBehaviour
{
    public static readonly List<FishRole> All = new List<FishRole>();

    [Tooltip("Pooper: normal fish that poops after eating.\n" +
             "Cleaner: eats poop and never poops.\n" +
             "Predator: hunts Cleaner fish (and poops after eating).")]
    [SerializeField] private FishRoleType role = FishRoleType.Pooper;

    public FishRoleType Role { get { return role; } }
    public FishController Controller { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<FishController>();
        ApplyRole();
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    /// <summary>
    /// Changes the role while the game is running (for example from a shop).
    /// </summary>
    public void SetRole(FishRoleType newRole)
    {
        role = newRole;
        ApplyRole();
    }

    private void ApplyRole()
    {
        SetBehaviour<FishPooper>(role != FishRoleType.Cleaner);
        SetBehaviour<PoopEater>(role == FishRoleType.Cleaner);
        SetBehaviour<PredatorHunter>(role == FishRoleType.Predator);
    }

    // Turns a behavior on (adding it if needed) or off
    private void SetBehaviour<T>(bool on) where T : MonoBehaviour
    {
        T component = GetComponent<T>();

        if (component == null)
        {
            if (!on) return;
            component = gameObject.AddComponent<T>();
        }

        component.enabled = on;
    }
}