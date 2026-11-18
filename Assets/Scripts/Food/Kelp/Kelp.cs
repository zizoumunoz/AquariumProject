using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Kelp : MonoBehaviour
{
    private static readonly int ReloadingParam = Animator.StringToHash("Reloading");

    [SerializeField] private KelpSO data;

    private FoodSource source;
    private Animator animator;
    private float reloadTimer;
    private bool reloading;

    private void Awake()
    {
        source = GetComponent<FoodSource>();
        animator = GetComponent<Animator>();

        source.fullnessRestore = data.fullnessPerBite;
        source.bitesLeft = data.bitesBeforeReload;
    }

    private void Update()
    {
        if (!reloading)
        {
            if (source.bitesLeft <= 0) StartReload();
            return;
        }

        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0f) Refresh();
    }

    private void StartReload()
    {
        reloading = true;
        reloadTimer = data.reloadTime;
        animator.SetBool(ReloadingParam, true);
    }

    private void Refresh()
    {
        reloading = false;
        source.bitesLeft = data.bitesBeforeReload;
        animator.SetBool(ReloadingParam, false);
    }
}