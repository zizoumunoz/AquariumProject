using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Kelp : MonoBehaviour
{
    [SerializeField] private KelpSO data;

    private FoodSource source;
    private float reloadTimer;


    private void Awake()
    {
        source = GetComponent<FoodSource>();
        source.fullnessRestore = data.fullnessPerBite;
        source.bitesLeft = data.bitesBeforeReload;
    }
    private void Update()
    {
        if (source.bitesLeft == 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer < 0)
            {
                reloadTimer = 0;
                Refresh();
            }
        }
    }

    private void Refresh()
    {
        source.bitesLeft = data.bitesBeforeReload;
        reloadTimer = data.reloadTime;
    }
}