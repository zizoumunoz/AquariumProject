using UnityEngine;

public class TankBounds : MonoBehaviour
{
    [SerializeField] private Collider2D floor;
    [SerializeField] private float bottomPadding;
    [SerializeField] private Collider2D ceiling;
    [SerializeField] private float topPadding;
    [SerializeField] private Collider2D leftWall;
    [SerializeField] private Collider2D rightWall;
    [SerializeField] private float xPadding;
    private void Start()
    {
        Camera cam = Camera.main;

        float cameraBottom = cam.transform.position.y - cam.orthographicSize;
        floor.transform.position = new(0, cameraBottom - (floor.bounds.size.y * 0.5f) + bottomPadding);

        float cameraTop = cam.transform.position.y + cam.orthographicSize;
        ceiling.transform.position = new(0, cameraTop + (ceiling.bounds.size.y * 0.5f) - topPadding);

        float cameraLeft = cam.transform.position.y - cam.orthographicSize * cam.aspect;
        leftWall.transform.position = new(cameraLeft - (leftWall.bounds.size.x * 0.5f) + xPadding, 0);

        float cameraRight = cam.transform.position.y + cam.orthographicSize * cam.aspect;
        rightWall.transform.position = new(cameraRight + (rightWall.bounds.size.x * 0.5f) - xPadding, 0);
    }
}