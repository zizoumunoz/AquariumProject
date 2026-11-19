using UnityEngine;

public class SwimWiggle : MonoBehaviour, IFishBehaviour
{
    [Header("Frequency (cycles speed)")]
    [SerializeField] private float slowFrequency = 2f;
    [SerializeField] private float fastFrequency = 11f;

    [Header("Amplitude (stretch amount)")]
    [SerializeField] private float slowAmplitude = 0.025f;
    [SerializeField] private float fastAmplitude = 0.065f;
    [SerializeField] private Transform visual;

    private FishStats stats;
    private Vector2 baseScale;
    private float phase;

    public void Init(FishStats stats)
    {
        baseScale = visual.localScale;
        phase = Random.Range(0f, Mathf.PI * 2f);
        this.stats = stats;
    }

    public void UpdateFish()
    {
        float speed = stats[StatsEnum.fishSpeed];

        phase = Mathf.Repeat(phase + Time.deltaTime * Mathf.Lerp(slowFrequency, fastFrequency, speed), Mathf.PI * 2f);
        float wiggle = Mathf.Sin(phase) * Mathf.Lerp(slowAmplitude, fastAmplitude, speed);

        visual.localScale = new Vector3(
            baseScale.x * (1f + wiggle),
            baseScale.y * (1f - wiggle * 0.5f),
            1f);
    }
}