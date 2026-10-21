using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fish collection book: opens from a button, shows page buttons and two page numbers
/// (left and right, like an open book) only while the book is open.
/// Put this on its own empty object (NOT on the book object, which starts disabled).
/// </summary>
public class FishCollectionBook : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private Button openButton;    // toggles the book
    [SerializeField] private Button closeButton;   // optional

    [Header("Pages")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [Tooltip("Top-left page number of the open book (#1, #3, #5...)")]
    [SerializeField] private TMP_Text leftPageText;
    [Tooltip("Top-right page number of the open book (#2, #4, #6...)")]
    [SerializeField] private TMP_Text rightPageText;
    [SerializeField] private string pagePrefix = "#";
    [Tooltip("How many times you can turn the page. Each one shows 2 pages (3 = pages 1 to 6).")]
    [SerializeField] private int totalPages = 3;

    [Header("Shown only while the book is open")]
    [Tooltip("Drag the previous button, next button and the slots parent here. The two page numbers are handled automatically.")]
    [SerializeField] private GameObject[] showWhileOpen;

    [Header("Squares (optional)")]
    [Tooltip("Leave empty for now. If used: the squares on ONE page.")]
    [SerializeField] private GameObject[] slots;
    [SerializeField] private int totalEntries = 12;

    private int currentPage; // 0 = first spread (#1 and #2)

    private int PageCount
    {
        get
        {
            if (slots != null && slots.Length > 0)
                return Mathf.Max(1, Mathf.CeilToInt((float)totalEntries / slots.Length));
            return Mathf.Max(1, totalPages);
        }
    }

    private void Awake()
    {
        if (openButton != null) openButton.onClick.AddListener(ToggleBook);
        if (closeButton != null) closeButton.onClick.AddListener(CloseBook);
        if (previousButton != null) previousButton.onClick.AddListener(PreviousPage);
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
    }

    private void Start()
    {
        SetBookVisible(false);
    }

    public void ToggleBook()
    {
        if (bookPanel == null) return;

        if (bookPanel.activeSelf) CloseBook();
        else OpenBook();
    }

    public void OpenBook()
    {
        currentPage = 0; // always opens on #1 and #2
        SetBookVisible(true);
        Refresh();
    }

    public void CloseBook()
    {
        SetBookVisible(false);
    }

    public void NextPage()
    {
        if (currentPage >= PageCount - 1) return;
        currentPage++;
        Refresh();
    }

    public void PreviousPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        Refresh();
    }

    private void SetBookVisible(bool visible)
    {
        if (bookPanel != null) bookPanel.SetActive(visible);

        if (leftPageText != null) leftPageText.gameObject.SetActive(visible);
        if (rightPageText != null) rightPageText.gameObject.SetActive(visible);

        if (showWhileOpen != null)
        {
            foreach (GameObject obj in showWhileOpen)
            {
                if (obj != null) obj.SetActive(visible);
            }
        }
    }

    private void Refresh()
    {
        // Squares 
        if (slots != null && slots.Length > 0)
        {
            int firstIndex = currentPage * slots.Length;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                    slots[i].SetActive(firstIndex + i < totalEntries);
            }
        }

        // Book-style numbers:
        int leftNumber = currentPage * 2 + 1;
        int rightNumber = currentPage * 2 + 2;

        if (leftPageText != null) leftPageText.text = pagePrefix + leftNumber;
        if (rightPageText != null) rightPageText.text = pagePrefix + rightNumber;

        if (previousButton != null) previousButton.interactable = currentPage > 0;
        if (nextButton != null) nextButton.interactable = currentPage < PageCount - 1;
    }
}