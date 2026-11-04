using UnityEngine;

public class SwimWiggle : MonoBehaviour
{
    [Header("Frequency (cycles speed)")]
    [SerializeField] private float slowFrequency = 2f;
    [SerializeField] private float fastFrequency = 11f;

    [Header("Amplitude (stretch amount)")]
    [SerializeField] private float slowAmplitude = 0.025f;
    [SerializeField] private float fastAmplitude = 0.065f;

    private IMotionSource motion;
    private Vector3 baseScale;
    private float phase;

    private void Awake()
    {
        motion = GetComponentInParent<IMotionSource>();
        baseScale = transform.localScale;
        phase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float speed = motion.NormalizedSpeed;

        phase = Mathf.Repeat(phase + Time.deltaTime * Mathf.Lerp(slowFrequency, fastFrequency, speed), Mathf.PI * 2f);
        float wiggle = Mathf.Sin(phase) * Mathf.Lerp(slowAmplitude, fastAmplitude, speed);

        transform.localScale = new Vector3(
            baseScale.x * (1f + wiggle),
            baseScale.y * (1f - wiggle * 0.5f),
            baseScale.z);
    }
}