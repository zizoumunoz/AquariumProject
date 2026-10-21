using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Money box at the top-center of the screen.
/// It shows the 'money' variable and refreshes whenever that value changes.
/// Creates itself when the game starts, so nothing needs to be set up in the scene.
/// </summary>
public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay Instance { get; private set; }

    /// <summary>
    /// Example: Connect here. Ex: MoneyDisplay.Instance.money += 10;  or  MoneyDisplay.Instance.AddMoney(10);
    /// </summary>
    public int money;

    private static readonly Vector2 BoxSize = new Vector2(280f, 70f);
    private const float TopMargin = 20f;

    private TextMeshProUGUI label;
    private int shownMoney = int.MinValue;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoCreate()
    {
        if (Instance != null) return;
        new GameObject("MoneyDisplay").AddComponent<MoneyDisplay>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        BuildUI();
    }
    private void Update()
    {
        /// Only touches the text when the value actually changed
        if (money == shownMoney) return;

        shownMoney = money;
        label.text = $"$ {money:N0}";
    }
    public void AddMoney(int amount)
    {
        money += amount;
    }
    private void BuildUI()
    {
        GameObject canvasObject = new GameObject("MoneyCanvas", typeof(Canvas), typeof(CanvasScaler));
        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject box = new GameObject("MoneyBox", typeof(RectTransform), typeof(Image));
        box.transform.SetParent(canvasObject.transform, false);

        RectTransform boxRect = box.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(0.5f, 1f);
        boxRect.anchorMax = new Vector2(0.5f, 1f);
        boxRect.pivot = new Vector2(0.5f, 1f);
        boxRect.anchoredPosition = new Vector2(0f, -TopMargin);
        boxRect.sizeDelta = BoxSize;

        box.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);

        GameObject textObject = new GameObject("MoneyText", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(box.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        label = textObject.GetComponent<TextMeshProUGUI>();
        label.alignment = TextAlignmentOptions.Center;
        label.fontSize = 40f;
        label.color = new Color32(255, 215, 0, 255);
    }
}