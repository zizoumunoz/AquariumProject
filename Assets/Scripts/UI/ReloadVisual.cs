using UnityEngine;

[RequireComponent(typeof(FoodReservoir), typeof(SpriteFrameAnimator))]
public class ReloadVisual : MonoBehaviour
{
    [SerializeField] private Sprite[] reloadingFrames;

    private FoodReservoir reservoir;
    private SpriteFrameAnimator animator;
    private Sprite[] readyFrames;

    private void Awake()
    {
        reservoir = GetComponent<FoodReservoir>();
        animator = GetComponent<SpriteFrameAnimator>();
        readyFrames = animator.Frames;
    }

    private void OnEnable()
    {
        reservoir.Depleted += ShowReloading;
        reservoir.Refilled += ShowReady;
    }

    private void OnDisable()
    {
        reservoir.Depleted -= ShowReloading;
        reservoir.Refilled -= ShowReady;
    }

    private void ShowReloading()
    {
        if (reloadingFrames.Length == 0) return;
        animator.SetFrames(reloadingFrames);
    }

    private void ShowReady()
    {
        animator.SetFrames(readyFrames);
    }
}