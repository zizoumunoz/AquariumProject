using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftClick : MonoBehaviour
{
    [SerializeField]
    GameObject foodToSpawn;
    public float topOffset = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);

            Camera cam = Camera.main;
            pos.y = cam.transform.position.y + cam.orthographicSize - topOffset;
            pos.z = 0f;
            Instantiate(foodToSpawn, pos, Quaternion.identity);
        }
    }
}
