using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlbumScript : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private FishDatabaseSO _fishDatabase;

    private const int _fishPerPage = 6;
    public int _currentPage = 1;
    private int _minimumPage = 1;
    private int _maximumPage = 10;

    [SerializeField] private TMP_Text _pageNumber;
    [SerializeField] private AlbumPanel[] _albumPanels;
    [SerializeField] private FishLorePanel _lorePanel;

    private void Awake()
    {
        // gets the maximum number of pages according to how many fishes there are
        _maximumPage = Mathf.Max(1, Mathf.CeilToInt(_fishDatabase.fishes.Count / (float)_fishPerPage));

        foreach(AlbumPanel panel in _albumPanels)
        {
            panel.Clicked += fish => _lorePanel.Show(fish);
        }
    }

    private void OnEnable()
    {
        FishCollection.FishDiscovered += OnFishDiscovered;
        SetPageNumberText();
        RefreshView();
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

            if (fishIndex >= _fishDatabase.fishes.Count)
            {
                _albumPanels[i].SetUndiscovered();
                continue;
            }


            FishSO fish = _fishDatabase.fishes[fishIndex];

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
