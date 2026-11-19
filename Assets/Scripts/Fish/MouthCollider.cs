using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    private FishBounds bounds;
    private void Start()
    {
        bounds = GetComponentInParent<FishBounds>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bounds.mouthCollisions.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bounds.mouthCollisions.Remove(collision);
    }
}
