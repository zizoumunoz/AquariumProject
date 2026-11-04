using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlbumScript : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private FishDatabaseSO _fishDatabase;


    private HashSet<FishSO> _unlockedFish = new();

    private const int _fishPerPage = 6;
    public int _currentPage = 1;
    private int _minimumPage = 1;
    private int _maximumPage = 10;

    [SerializeField] private TMP_Text _pageNumber;

    [SerializeField] private AlbumPanel[] _albumPanels;

    private void Start()
    {
        //RefreshView();

        for(int i = 0; i < _fishPerPage; i++)
        {
            _unlockedFish.Add(_fishDatabase.fishes[i]);
            _albumPanels[i].SetFish(_fishDatabase.fishes[i]);
        }
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

        for (int i = 0; i < _fishPerPage; i++)
        {
            int fishIndex = pageStartIndex + i;

            // prevents from accessing fish not in database

            if (fishIndex >= _fishDatabase.fishes.Count)
            {
                _albumPanels[i].SetUndiscovered();
                continue;
            }


            FishSO fish = _fishDatabase.fishes[fishIndex];

            _albumPanels[i].SetFish(fish);

            //if (_unlockedFish.Contains(fish))
            //{
            //    _albumPanels[i].SetFish(fish);
            //}

            //else
            //{
            //    _albumPanels[i].SetUndiscovered();
            //}
        }
    }
}
