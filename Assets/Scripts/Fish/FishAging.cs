using UnityEngine;

public class FishAging : MonoBehaviour, IFishBehaviour
{
    public FishStats stats;
    public FishVisualController vc;
    public float timeToGrow = 0;
    public bool isKid = false;

    public int Order => 0;

    public void Init(FishStats stats)
    {
        this.stats = stats;
        vc = stats.GetBehaviour<FishVisualController>();

        UpdateAge(stats.age);
    }

    public void UpdateFish()
    {
        if (timeToGrow < 0)
        {
            return;
        }
        timeToGrow -= Time.deltaTime;
        if (timeToGrow < 0)
        {
            UpdateAge(Age.Adult);
        }
    }
    public void UpdateAge(Age age)
    {
        if (age == Age.Young)
        {
            stats.statsMultiplier[StatsEnum.baseSize] *= stats[StatsEnum.youngSizeMultiplier];
            stats.priceMultiplier *= 0.5f;
            timeToGrow = stats[StatsEnum.ageTimer];
            isKid = true;
        }
        else
        {
            if (isKid)
            {
                stats.priceMultiplier /= 0.5f;
                stats.statsMultiplier[StatsEnum.baseSize] /= stats[StatsEnum.youngSizeMultiplier];
            }
            isKid = false;
        }
        vc.transform.localScale = new Vector2(stats[StatsEnum.baseSize], stats[StatsEnum.baseSize]);
    }
}
