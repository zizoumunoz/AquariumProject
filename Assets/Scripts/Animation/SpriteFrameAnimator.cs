using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFrameAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 3f;
    [SerializeField] private bool randomStartFrame;

    private IMotionSource motion;
    private SpriteRenderer sr;
    private float timer;
    private int index;

    public Sprite[] Frames { get { return frames; } }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        motion = GetComponentInParent<IMotionSource>();

        if (randomStartFrame && frames.Length > 0)
        {
            index = Random.Range(0, frames.Length);
            sr.sprite = frames[index];
        }
    }

    public void SetFrames(Sprite[] newFrames)
    {
        frames = newFrames;
        if (frames.Length == 0) return;

        index %= frames.Length;
        sr.sprite = frames[index];
    }

    private void Update()
    {
        if (frames.Length == 0) return;

        float speed = motion != null ? motion.NormalizedSpeed : 1f;
        if (speed <= 0f) return;

        timer += Time.deltaTime * speed;

        float frameTime = 1f / framesPerSecond;
        if (timer < frameTime) return;

        timer -= frameTime;
        index = (index + 1) % frames.Length;
        sr.sprite = frames[index];
    }
}