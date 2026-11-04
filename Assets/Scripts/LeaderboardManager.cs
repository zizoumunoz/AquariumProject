using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [Serializable]
    public class PlayerData
    {
        public string playerId;
        public string username;
        public Sprite picture;
        public long money;
    }

    [Serializable]
    public class RowUI
    {
        public Image picture;
        public TMP_Text usernameText;
        public TMP_Text moneyText;

        public void Display(PlayerData player, Sprite defaultPicture)
        {
            bool occupied = player != null;

            if (picture != null)
            {
                picture.gameObject.SetActive(occupied);
                picture.sprite = occupied
                    ? player.picture ?? defaultPicture
                    : null;

                picture.enabled = occupied && picture.sprite != null;
                picture.preserveAspect = true;
            }

            if (usernameText != null)
            {
                usernameText.gameObject.SetActive(occupied);
                usernameText.richText = false;
                usernameText.text = occupied ? player.username : "";
            }

            if (moneyText != null)
            {
                moneyText.gameObject.SetActive(occupied);
                moneyText.text = occupied
                    ? "$" + player.money.ToString(
                        "N0", CultureInfo.InvariantCulture)
                    : "";
            }
        }
    }

    [Header("Visible rows, from top to bottom")]
    [SerializeField] private RowUI[] rows = new RowUI[6];

    [Header("Players")]
    [SerializeField]
    private List<PlayerData> players =
        new List<PlayerData>();

    [Header("Open / Close")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("Optional")]
    [SerializeField] private Sprite defaultPicture;
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private int currentPage;

    private readonly List<PlayerData> sortedPlayers =
        new List<PlayerData>();

    private int PageCount => rows == null || rows.Length == 0
        ? 1
        : Mathf.Max(
            1,
            Mathf.CeilToInt(sortedPlayers.Count / (float)rows.Length));

    private void Awake()
    {
        if (openButton != null)
            openButton.onClick.AddListener(OpenLeaderboard);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseLeaderboard);
    }

    private void OnEnable()
    {
        RefreshLeaderboard();
        UpdateButtonVisibility();
    }

    private void OnDestroy()
    {
        if (openButton != null)
            openButton.onClick.RemoveListener(OpenLeaderboard);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseLeaderboard);
    }

    public void OpenLeaderboard()
    {
        if (leaderboardPanel == null)
        {
            Debug.LogWarning(
                "Assign Leaderboard Panel in the Inspector.", this);
            return;
        }

        leaderboardPanel.SetActive(true);
        RefreshLeaderboard();
        UpdateButtonVisibility();
    }

    public void CloseLeaderboard()
    {
        if (leaderboardPanel == null)
        {
            Debug.LogWarning(
                "Assign Leaderboard Panel in the Inspector.", this);
            return;
        }

        leaderboardPanel.SetActive(false);
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        if (leaderboardPanel == null) return;

        bool isOpen = leaderboardPanel.activeSelf;

        if (openButton != null)
            openButton.gameObject.SetActive(!isOpen);

        if (closeButton != null)
            closeButton.gameObject.SetActive(isOpen);
    }

    public void RefreshLeaderboard()
    {
        sortedPlayers.Clear();

        foreach (PlayerData player in players)
        {
            if (player != null)
                sortedPlayers.Add(player);
        }

        sortedPlayers.Sort((a, b) =>
        {
            int moneyOrder = b.money.CompareTo(a.money);

            if (moneyOrder != 0)
                return moneyOrder;

            int nameOrder = string.Compare(
                a.username,
                b.username,
                StringComparison.Ordinal);

            return nameOrder != 0
                ? nameOrder
                : string.Compare(
                    a.playerId,
                    b.playerId,
                    StringComparison.Ordinal);
        });

        ShowPage();
    }

    public void NextPage()
    {
        currentPage = Mathf.Min(currentPage + 1, PageCount - 1);
        ShowPage();
    }

    public void PreviousPage()
    {
        currentPage = Mathf.Max(currentPage - 1, 0);
        ShowPage();
    }

    private void ShowPage()
    {
        currentPage = Mathf.Clamp(currentPage, 0, PageCount - 1);

        if (rows != null)
        {
            int startIndex = currentPage * rows.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                int playerIndex = startIndex + i;

                rows[i]?.Display(
                    playerIndex < sortedPlayers.Count
                        ? sortedPlayers[playerIndex]
                        : null,
                    defaultPicture);
            }
        }

        if (pageText != null)
        {
            pageText.text = sortedPlayers.Count == 0
                ? "No players"
                : $"Page {currentPage + 1} / {PageCount}";
        }

        bool hasRows = rows != null && rows.Length > 0;

        if (previousButton != null)
        {
            previousButton.interactable =
                hasRows && currentPage > 0;
        }

        if (nextButton != null)
        {
            nextButton.interactable =
                hasRows && currentPage < PageCount - 1;
        }
    }

    // Pass the player's total money, not the amount earned.
    public void AddOrUpdatePlayer(
        string playerId,
        string username,
        Sprite picture,
        long money)
    {
        if (string.IsNullOrWhiteSpace(playerId))
        {
            Debug.LogWarning(
                "Leaderboard: playerId cannot be empty.", this);
            return;
        }

        PlayerData player = players.Find(
            p => p != null && p.playerId == playerId);

        if (player == null)
        {
            player = new PlayerData { playerId = playerId };
            players.Add(player);
        }

        player.username = username;
        player.picture = picture;
        player.money = money;

        RefreshLeaderboard();
    }
}