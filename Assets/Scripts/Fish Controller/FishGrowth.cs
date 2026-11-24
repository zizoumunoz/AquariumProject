using UnityEngine;

public class FishGrowth : MonoBehaviour
{
    FishController fish;
    private bool ready;

    [SerializeField] private float timer;
    private Age age = Age.Young;

    public Age CurrentAge { get { return age; } }
    public float Timer { get { return timer; } }

    private void Awake()
    {
        fish = GetComponent<FishController>();
    }

    public void Setup(float savedTimer)
    {
        timer = savedTimer != -1f ? savedTimer : fish.Data.fishStatus.ageTime * fish.statusMultiplier.ageTime;
        ready = true;
    }

    private void Update()
    {
        if (!ready || age != Age.Young) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        age = Age.Young;
        transform.localScale = Vector3.one * fish.Size;
        GetComponent<FoodSource>().size = fish.Size; 

    }

}
