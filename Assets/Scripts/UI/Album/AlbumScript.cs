using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class AlbumScript : MonoBehaviour
{
    [Header("Hidden while the album is open")]
    [SerializeField] private GameObject[] _hideWhileOpen;
    [SerializeField] private GameObject _emptyMessage;  // for when there's no fish implemented in that aquarium yet
    public static List<List<bool>> _discoveredFish = new();

    private readonly List<FishSO> _tankFish = new();
    private int _currentTank = 0;


    private const int _fishPerPage = 6;
    public int _currentPage = 1;
    private int _minimumPage = 1;
    private int _maximumPage = 10;

    [SerializeField] private TMP_Text _pageNumber;
    [SerializeField] private AlbumPanel[] _albumPanels;
    [SerializeField] private FishLorePanel _lorePanel;

    public static void SyncFromCollection()
    {
        List<TankSO> tanks = TankManager.instance.database.tankSOs;
        _discoveredFish.Clear();
        foreach (TankSO tank in tanks)
        {
            List<bool> tankFlags = new();
            foreach(FishSO fish in tank.fishes)
            {
                tankFlags.Add(FishCollection.IsDiscovered(fish));
            }
            _discoveredFish.Add(tankFlags);
        }
    }

    // mark saved fish as discovered
    public static void ApplyToCollection()
    {
        List<TankSO> tanks = TankManager.instance.database.tankSOs;
        for (int t = 0; t < _discoveredFish.Count && t < tanks.Count; t++)
        {
            for (int f = 0; f < _discoveredFish[t].Count && f < tanks[t].fishes.Count; f++)
            {
                if (_discoveredFish[t][f])
                {
                    FishCollection.Discover(tanks[t].fishes[f]);
                }
            }
        }
    }

    private void Awake()
    {
        foreach (AlbumPanel panel in _albumPanels)
        {
            panel.Clicked += fish => _lorePanel.Show(fish);
        }
    }

    public void SetTank(int tankIndex)
    {
        List<TankSO> tanks = TankManager.instance.database.tankSOs;
        _currentTank = Mathf.Clamp(tankIndex, 0, tanks.Count - 1);
        TankSO tank = tanks[_currentTank];

        // Load this tanks fish
        _tankFish.Clear();
        _tankFish.AddRange(tank.fishes);

        _maximumPage = Mathf.Max(1, Mathf.CeilToInt(_tankFish.Count / (float)_fishPerPage));
        _currentPage = _minimumPage;

        bool isEmpty = _tankFish.Count == 0;
        if (_emptyMessage != null)
        {
            _emptyMessage.SetActive(isEmpty);
        }
        foreach (AlbumPanel panel in _albumPanels)
        {
            panel.gameObject.SetActive(!isEmpty);
        }

        // update screen
        SetPageNumberText();
        RefreshView();
    }

    private void OnEnable()
    {
        // disable other UI elements
        foreach (GameObject obj in _hideWhileOpen)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        FishCollection.FishDiscovered += OnFishDiscovered;
        SetTank((int)TankManager.instance.database.activeTank.type);
    }

    private void OnDisable()
    {
        FishCollection.FishDiscovered -= OnFishDiscovered;

        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        // hide non related UI items
        foreach (GameObject obj in _hideWhileOpen)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnFishDiscovered(FishSO fish)
    {
        RefreshView();
    }

    private void SetPageNumberText()
    {
        _pageNumber.text = _currentPage.ToString();
    }

    public void SetNextPage()
    {
        if (_currentPage < _maximumPage)
        {
            ++_currentPage;
            SetPageNumberText();
            RefreshView();
        }
    }

    public void SetPreviousPage()
    {
        if (_currentPage > _minimumPage)
        {
            --_currentPage;
            SetPageNumberText();
            RefreshView();
        }
    }

    private void RefreshView()
    {
        int pageStartIndex = (_currentPage - 1) * _fishPerPage;

        for (int i = 0; i < _albumPanels.Length; i++)
        {
            int fishIndex = pageStartIndex + i;

            // prevents from accessing fish not in database

            if (fishIndex >= _tankFish.Count)
            {
                _albumPanels[i].SetUndiscovered();
                continue;
            }

            FishSO fish = _tankFish[fishIndex];

            // show fish only if player obtained
            if (FishCollection.IsDiscovered(fish))
            {
                _albumPanels[i].SetFish(fish);
            }
            else
            {
                _albumPanels[i].SetUndiscovered();
            }
        }
    }
}
