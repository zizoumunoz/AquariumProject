using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine.VFX;
using System.Security.Cryptography;

public class AlgaeVisuals : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite _smallAlgae;
    [SerializeField] private Sprite _mediumAlgae;
    [SerializeField] private Sprite _largeAlgae;

    [Header("Spawning")]
    [SerializeField] private int _maxSpots = 40;
    [SerializeField] private float _edgeMargin = 0.2f;
    [SerializeField] private string _sortingLayer = "Default";
    [SerializeField] private int _sortingOrder = -5;
    [SerializeField] private Vector2 _scaleRange = new Vector2(0.8f, 1.2f);

    private readonly List<SpriteRenderer> _spots = new();
    private Tank _currentTank;
    private Bounds _currentBounds;

    private void Update()
    {
        if (TankManager.instance == null)
        {
            return;
        }

        Tank tank = TankManager.instance.database.activeTank;
        if (tank == null)
        {
            return;
        }

        Bounds tankBounds = TankManager.instance.sr.bounds;

        if (tank != _currentTank || tankBounds != _currentBounds)
        {
            Clear();
            _currentTank = tank;
            _currentBounds = tankBounds;
        }

        float algae = Mathf.Clamp01(tank.currentAttibutes.moldLevel / 100f);
        int target = Mathf.FloorToInt(algae * _maxSpots);

        while (_spots.Count < target)
        {
            SpawnSpot(algae);
        }
        while (_spots.Count > target)
        {
            RemoveSpot();
        }
    }

    private void SpawnSpot(float algae)
    {
        Sprite sprite = PickSprite(algae);
        float radius = sprite.bounds.extents.x * _scaleRange.y;
        float min = _edgeMargin + radius;

        GameObject obj = new GameObject("Algae");
        obj.transform.SetParent(transform);
        obj.transform.position = new Vector3(
            Random.Range(_currentBounds.min.x + min, _currentBounds.max.x - min),
            Random.Range(_currentBounds.min.y + min, _currentBounds.max.y - min), 0f
            );
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        obj.transform.localScale = Vector3.one * Random.Range(_scaleRange.x, _scaleRange.y);

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

        sr.sprite = sprite;
        sr.sortingLayerName = _sortingLayer;
        sr.sortingOrder = _sortingOrder;
        _spots.Add(sr);
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
            _spots.Clear();
        }
    }
}
