using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFrameAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 3f;

    private IMotionSource motion;
    private SpriteRenderer sr;
    private float timer;
    private int index;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        motion = GetComponentInParent<IMotionSource>();
    }

    private void Update()
    {
        if (frames.Length == 0 || motion.NormalizedSpeed <= 0f) return;

        timer += Time.deltaTime * motion.NormalizedSpeed;

        float frameTime = 1f / framesPerSecond;
        if (timer < frameTime) return;

        timer -= frameTime;
        index = (index + 1) % frames.Length;
        sr.sprite = frames[index];
    }
}