using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fish collection book: opens from a button, shows page buttons, two page numbers
/// (left and right, like an open book) and section tabs only while the book is open.
/// Clicking a tab jumps straight to the first pages of that section.
/// Put this on its own empty object (NOT on the book object, which starts disabled).
/// </summary>
public class FishCollectionBook : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private Button openButton;    // toggles the book
    [SerializeField] private Button closeButton;  

    [Header("Pages")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [Tooltip("Top-left page number of the open book (#1, #3, #5...)")]
    [SerializeField] private TMP_Text leftPageText;
    [Tooltip("Top-right page number of the open book (#2, #4, #6...)")]
    [SerializeField] private TMP_Text rightPageText;
    [SerializeField] private string pagePrefix = "#";
    [Tooltip("Used only when there are no tabs. Each one shows 2 pages (3 = pages 1 to 6).")]
    [SerializeField] private int totalPages = 3;

    [Header("Tabs (right side of the book)")]
    [Tooltip("In order: Fresh water, Salty water, Candy water, Giant fish.")]
    [SerializeField] private Button[] tabs;
    [Tooltip("How many open spreads each tab has. 2 = 4 pages per tab (#1 to #4, #5 to #8...).")]
    [SerializeField] private int spreadsPerTab = 2;
    [SerializeField] private Color normalTabColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color selectedTabColor = Color.white;

    [Header("Shown only while the book is open")]
    [Tooltip("Drag the previous button, next button and the slots parent here. Page numbers and tabs are handled automatically.")]
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
            if (tabs != null && tabs.Length > 0)
                return Mathf.Max(1, tabs.Length * Mathf.Max(1, spreadsPerTab));

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

        if (tabs != null)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                int tabIndex = i; // copy, so each button remembers its own number
                if (tabs[i] != null)
                    tabs[i].onClick.AddListener(() => GoToTab(tabIndex));
            }
        }
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

    /// <summary>
    /// Jumps to the first pages of a tab (0 = Fresh water, 1 = Salty water, ...).
    /// </summary>
    public void GoToTab(int tabIndex)
    {
        currentPage = Mathf.Clamp(tabIndex * Mathf.Max(1, spreadsPerTab), 0, PageCount - 1);
        Refresh();
    }

    private void SetBookVisible(bool visible)
    {
        if (bookPanel != null) bookPanel.SetActive(visible);

        if (leftPageText != null) leftPageText.gameObject.SetActive(visible);
        if (rightPageText != null) rightPageText.gameObject.SetActive(visible);

        if (tabs != null)
        {
            foreach (Button tab in tabs)
            {
                if (tab != null) tab.gameObject.SetActive(visible);
            }
        }

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
        // Squares (only if you assigned some)
        if (slots != null && slots.Length > 0)
        {
            int firstIndex = currentPage * slots.Length;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                    slots[i].SetActive(firstIndex + i < totalEntries);
            }
        }

        // Book-style numbers
        int leftNumber = currentPage * 2 + 1;
        int rightNumber = currentPage * 2 + 2;

        if (leftPageText != null) leftPageText.text = pagePrefix + leftNumber;
        if (rightPageText != null) rightPageText.text = pagePrefix + rightNumber;

        if (previousButton != null) previousButton.interactable = currentPage > 0;
        if (nextButton != null) nextButton.interactable = currentPage < PageCount - 1;

        UpdateTabs();
    }

    // Highlights the tab of the section you are currently reading
    private void UpdateTabs()
    {
        if (tabs == null) return;

        int selected = currentPage / Mathf.Max(1, spreadsPerTab);

        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i] == null || tabs[i].image == null) continue;
            tabs[i].image.color = (i == selected) ? selectedTabColor : normalTabColor;
        }
    }
}