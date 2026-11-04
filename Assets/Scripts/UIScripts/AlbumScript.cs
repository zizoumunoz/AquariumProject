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

    [SerializeField] private AlbumPanelScript[] _albumPanels;

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
        }
    }

    public void SetPreviousPage()
    {
        if (_currentPage > _minimumPage)
        {
            --_currentPage;
            SetPageNumberText();
        }
    }

    private void RefreshView()
    {
        for (int i = 0; i < _fishPerPage; i++)
        {
            
        }
    }
}
