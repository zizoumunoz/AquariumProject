using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Poop : MonoBehaviour
{
    public static readonly List<Poop> All = new List<Poop>();

    [SerializeField] private float sinkSpeed = 0.5f;

    private Rigidbody2D rb;
    private bool landed;

    public bool IsLanded { get { return landed; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    private void FixedUpdate()
    {
        if (landed) return;
        rb.linearVelocity = Vector2.down * sinkSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (landed) return;
        if (!collision.gameObject.CompareTag("Bottom")) return;

        landed = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Consume()
    {
        Destroy(gameObject);
    }
}