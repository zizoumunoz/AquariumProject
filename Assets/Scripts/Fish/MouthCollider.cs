using UnityEngine;

public class MouthCollider : MonoBehaviour, IFishBehaviour
{
    private FishBounds bounds;

    public int Order => 0;

    public void Init(FishStats stats)
    {
        bounds = stats.GetBehaviour<FishBounds>();
    }

    public void UpdateFish(){}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bounds.mouthCollisions.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bounds.mouthCollisions.Remove(collision);
    }
}
