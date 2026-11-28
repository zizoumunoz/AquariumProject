using UnityEngine;
using System.Collections.Generic;

public class AlgaeVisuals : MonoBehaviour
{
    public static AlgaeVisuals instance;

    [Header("TankBounds")]
    [SerializeField] private Collider2D _wallLeft;
    [SerializeField] private Collider2D _wallRight;
    [SerializeField] private Collider2D _wallTop;
    [SerializeField] private Collider2D _wallBottom;

    [Header("Eating")]
    [SerializeField] private float _fullnessPerSpot = 20f;

    /// <summary>
    /// How much algaeLevel drops when a spot is eaten. Divide by 100 to make it a percentage based on the max number of possible spots.
    /// </summary>
    private float AlgaeLevelPerSpot { get { return 100f / _maxSpots; } }

    [Header("Sprites")]
    [SerializeField] private Sprite _smallAlgae;
    [SerializeField] private Sprite _mediumAlgae;
    [SerializeField] private Sprite _largeAlgae;

    [Header("Spawning")]
    [SerializeField] private SpriteRenderer _algaePrefab;
    [SerializeField] public int _maxSpots = 40;
    [SerializeField] private float _edgeMargin = 0.2f;
    [SerializeField] private Vector2 _scaleRange = new Vector2(0.8f, 1.2f);

    private readonly List<SpriteRenderer> _spots = new();
    private Tank _currentTank;
    private Bounds _currentBounds;

    private float lastSize = Settings.Instance.algaeSize;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        for (int i = 0; i < _spots.Count; i++)
        {
            if (_spots[i] == null)
            {
                _spots.RemoveAt(i);
                i--;
            }
        }
        Tank tank = TankManager.instance.database.activeTank;

        Bounds tankBounds = GetPlayAreaBounds();

        if (tank != _currentTank || tankBounds != _currentBounds)
        {
            Clear();
            _currentTank = tank;
            _currentBounds = tankBounds;
        }

        float algae = Mathf.Clamp01(tank.currentAttibutes.algaeLevel / 100f);
        int target = Mathf.FloorToInt(algae * _maxSpots);

        while (_spots.Count < target)
        {
            SpawnSpot(algae);
        }
        while (_spots.Count > target)
        {
            RemoveSpot();
        }

        if (lastSize != Settings.Instance.algaeSize)
        {
            float aspect = Settings.Instance.algaeSize / lastSize;
            for (int i = 0; i < _spots.Count; i++)
            {
                _spots[i].transform.localScale *= aspect;
            }

            lastSize = Settings.Instance.algaeSize;
        }
    }

    private void SpawnSpot(float algae)
    {
        Sprite sprite = PickSprite(algae);
        float radius = sprite.bounds.extents.x * _scaleRange.y;
        float min = _edgeMargin + radius;

        SpriteRenderer sr = Instantiate(_algaePrefab, transform);
        sr.transform.position = new Vector3(
            Random.Range(_currentBounds.min.x + min, _currentBounds.max.x - min),
            Random.Range(_currentBounds.min.y + min, _currentBounds.max.y - min),
            0f
            );

        sr.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        sr.transform.localScale = Vector3.one * Random.Range(_scaleRange.x, _scaleRange.y) * Settings.Instance.algaeSize / 10;
        sr.sprite = sprite;

        sr.GetComponent<CircleCollider2D>().radius = sprite.bounds.extents.x;
        FoodSource source = sr.GetComponent<FoodSource>();
        source.fullnessRestore = _fullnessPerSpot;
        source.Eaten += OnSpotEaten;


        _spots.Add(sr);
    }

    private void OnSpotEaten(FoodSource source)
    {
        if (source.bitesLeft > 0)
        {
            return;
        }

        _spots.Remove(source.GetComponent<SpriteRenderer>());
        Destroy(source.gameObject);

        TankAttribute attributes = _currentTank.currentAttibutes;
        attributes.algaeLevel = Mathf.Max(0f, attributes.algaeLevel - AlgaeLevelPerSpot);
    }

    // more mold means big spots likelier
    private Sprite PickSprite(float algae)
    {
        float roll = Random.value;
        if (roll < algae * 0.4f)
        {
            return _largeAlgae;
        }
        if (roll < algae * 0.4f + 0.35f)
        {
            return _mediumAlgae;
        }
        return _smallAlgae;
    }

    private void RemoveSpot()
    {
        SpriteRenderer last = _spots[^1];
        _spots.RemoveAt(_spots.Count - 1);
        Destroy(last.gameObject);
    }

    private void Clear()
    {
        foreach (SpriteRenderer sr in _spots)
        {
            Destroy(sr.gameObject);
        }
        _spots.Clear();
    }
    private Bounds GetPlayAreaBounds()
    {
        Vector3 min = new Vector3(_wallLeft.bounds.max.x, _wallBottom.bounds.max.y, 0f);
        Vector3 max = new Vector3(_wallRight.bounds.min.x, _wallTop.bounds.min.y, 0f);

        Bounds b = new Bounds();

        b.SetMinMax(min, max);

        return b;
    }
}
