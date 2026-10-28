using UnityEngine;

/// <summary>
/// Creates itself when the game starts and makes sure every fish has a FishRole.
/// Fish that don't have one get the default role (Pooper), which also gives them
/// the poop behavior. Works in every scene and for fish spawned later (for example
/// bought in a shop). To give a fish another role, add FishRole to its prefab
/// and pick the role in the Inspector.
/// </summary>
public class FishPoopSetup : MonoBehaviour
{
    private const float CheckInterval = 0.5f;

    private float timer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoCreate()
    {
        if (FindAnyObjectByType<FishPoopSetup>() != null) return;

        GameObject go = new GameObject("FishPoopSetup");
        DontDestroyOnLoad(go);
        go.AddComponent<FishPoopSetup>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = CheckInterval;

        foreach (FishController fish in FishController.All)
        {
            if (fish == null) continue;
            if (fish.GetComponent<FishRole>() == null)
                fish.gameObject.AddComponent<FishRole>();
        }
    }
}