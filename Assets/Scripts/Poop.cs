using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Poop dropped by fish. It sinks until it reaches the floor (the object tagged "Bottom"),
/// then stays there until a fish with PoopEater eats it.
/// Poop.Spawn() works with no prefab. For custom art, make a prefab with a SpriteRenderer
/// and this script, and save it as Assets/Resources/Poop.prefab.
/// </summary>
public class Poop : MonoBehaviour
{
    public static readonly List<Poop> All = new List<Poop>();

    private const int MaxPoopInTank = 40;
    private const string PrefabName = "Poop";

    [SerializeField] private float fullnessRestore = 25f;
    [SerializeField] private float sinkSpeed = 0.4f;
    [Tooltip("How far above the top of the 'Bottom' object the poop rests.")]
    [SerializeField] private float floorOffset = 0.1f;
    [Tooltip("Seconds until it disappears by itself. 0 = stays until a fish eats it.")]
    [SerializeField] private float lifetime = 0f;
    [SerializeField] private float fadeTime = 3f;

    public float FullnessRestore { get { return fullnessRestore; } }

    private static Sprite placeholderSprite;

    private SpriteRenderer sr;
    private Camera cam;
    private float floorY;
    private float age;

    /// <summary>
    /// Creates one poop at a position. Removes the oldest one if the tank is too messy.
    /// </summary>
    public static Poop Spawn(Vector3 position)
    {
        while (All.Count >= MaxPoopInTank)
        {
            Destroy(All[0].gameObject);
            All.RemoveAt(0);
        }

        Poop prefab = Resources.Load<Poop>(PrefabName);
        if (prefab != null)
            return Instantiate(prefab, position, Quaternion.identity);

        GameObject go = new GameObject("Poop");
        go.transform.position = position;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = GetPlaceholderSprite();
        renderer.sortingOrder = 1;

        return go.AddComponent<Poop>();
    }

    public static Poop FindNearest(Vector2 from)
    {
        Poop nearest = null;
        float best = float.MaxValue;

        foreach (Poop poop in All)
        {
            float d = Vector2.Distance(from, poop.transform.position);
            if (d < best)
            {
                best = d;
                nearest = poop;
            }
        }
        return nearest;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    private void Start()
    {
        // Fish can't swim lower than a little above the bottom of the screen,
        // so never rest the poop below that or it can't be reached.
        float camBottom = cam.transform.position.y - cam.orthographicSize;
        floorY = camBottom + 0.7f;

        // Use the real floor from the scene (same "Bottom" tag Food.cs uses)
        GameObject bottom = FindBottom();
        if (bottom != null)
        {
            Collider2D col = bottom.GetComponent<Collider2D>();
            if (col != null)
                floorY = Mathf.Max(floorY, col.bounds.max.y + floorOffset);
        }
    }

    private void Update()
    {
        if (lifetime > 0f)
        {
            age += Time.deltaTime;
            if (age >= lifetime)
            {
                Destroy(gameObject);
                return;
            }

            if (sr != null && age > lifetime - fadeTime)
            {
                Color c = sr.color;
                c.a = Mathf.Clamp01((lifetime - age) / fadeTime);
                sr.color = c;
            }
        }

        Vector3 p = transform.position;
        if (p.y > floorY)
        {
            p.y = Mathf.Max(floorY, p.y - sinkSpeed * Time.deltaTime);
            transform.position = p;
        }
    }

    private static GameObject FindBottom()
    {
        try
        {
            return GameObject.FindWithTag("Bottom");
        }
        catch (UnityException)
        {
            return null; // the tag doesn't exist in this project
        }
    }

    // A small brown pixel blob, used when there is no Resources/Poop prefab
    private static Sprite GetPlaceholderSprite()
    {
        if (placeholderSprite != null) return placeholderSprite;

        const int size = 8;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;

        Color brown = new Color(0.45f, 0.28f, 0.12f, 1f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - 3.5f;
                float dy = y - 3.5f;
                texture.SetPixel(x, y, dx * dx + dy * dy <= 12f ? brown : Color.clear);
            }
        }

        texture.Apply();
        placeholderSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 16f);
        return placeholderSprite;
    }
}