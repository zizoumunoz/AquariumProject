using TMPro;
using UnityEngine;

public class AlbumScript : MonoBehaviour
{
    public int _currentPage = 1;
    private int _minimumPage = 1;
    private int _maximumPage = 10;
    [SerializeField] private TMP_Text _pageNumber;


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
}

