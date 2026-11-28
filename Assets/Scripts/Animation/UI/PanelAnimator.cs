using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelAnimator : MonoBehaviour
{
    private bool openning = true;
    private bool open = false;
    [Header("Timer")]
    public AnimationCurve animationSmoothness = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private float timer = 0;
    [Header("Opacity")]
    public bool opacity = true;
    public CanvasGroup group;
    [Range(0.0f, 1.0f)] public float closedOpacity = 0f;
    [Range(0.0f, 1.0f)] public float openOpacity = 1f;
    [Header("Position")]
    public bool position = true;
    public Vector2 closedPosition = Vector2.zero;
    public Vector2 openPosition = Vector2.zero;
    [Header("Scale")]
    public bool scale = true;
    public Vector2 closedScale = Vector2.one;
    public Vector2 openScale = Vector2.one;
    [Header("Extras")]
    public bool extras = false;
    private Vector2 previousMoneyPosition;
    public Vector2 moneyPosition;
    private bool previousShowButtons;
    public bool showButtons;

    [SerializeField] private GameObject blockScreenPanel;

    void Update()
    {
        if (open)
        {
            return;
        }
        Vector2 screenSize = new(Camera.main.pixelWidth, Camera.main.pixelHeight);
        if (openning)
        {
            timer += Time.deltaTime;
            if (timer > Settings.Instance.animationDuration)
            {
                blockScreenPanel.SetActive(false);
                if (opacity)
                {
                    group.alpha = openOpacity;
                }
                if (position)
                {
                    transform.position = new(openPosition.x * screenSize.x, openPosition.y * screenSize.y);
                }
                if (scale)
                {
                    transform.localScale = openScale;
                }
                if (extras)
                {
                    ScreenManager.instance.moneyDisplay.position = new(moneyPosition.x * screenSize.x, moneyPosition.y * screenSize.y);
                }
                timer = Settings.Instance.animationDuration;
                open = true;
                return;
            }

            float smoothedTimer = animationSmoothness.Evaluate(timer / Settings.Instance.animationDuration);

            if (opacity)
            {
                group.alpha = Mathf.LerpUnclamped(closedOpacity, openOpacity, smoothedTimer);
            }
            if (position)
            {
                transform.position = Vector2.LerpUnclamped(new(closedPosition.x * screenSize.x, closedPosition.y * screenSize.y),
                    new(openPosition.x * screenSize.x, openPosition.y * screenSize.y), smoothedTimer);
            }
            if (scale)
            {
                transform.localScale = Vector2.LerpUnclamped(closedScale, openScale, smoothedTimer);
            }
            if (extras)
            {
                ScreenManager.instance.moneyDisplay.position = Vector2.LerpUnclamped(
                    new(previousMoneyPosition.x, previousMoneyPosition.y),
                    new(moneyPosition.x * screenSize.x, moneyPosition.y * screenSize.y), smoothedTimer);
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                blockScreenPanel.SetActive(false);
                if (extras)
                {
                    for (int i = 0; i < ScreenManager.instance.buttons.Length; i++)
                    {
                        ScreenManager.instance.buttons[i].SetActive(previousShowButtons);
                    }
                }
                gameObject.SetActive(false);
                timer = 0;
                return;
            }

            float smoothedTimer = animationSmoothness.Evaluate(timer / Settings.Instance.animationDuration);

            if (opacity)
            {
                group.alpha = Mathf.LerpUnclamped(closedOpacity, openOpacity, smoothedTimer);
            }
            if (position)
            {
                transform.position = Vector2.LerpUnclamped(new(closedPosition.x * screenSize.x, closedPosition.y * screenSize.y),
                    new(openPosition.x * screenSize.x, openPosition.y * screenSize.y), smoothedTimer);
            }
            if (scale)
            {
                transform.localScale = Vector2.LerpUnclamped(closedScale, openScale, smoothedTimer);
            }
            if (extras)
            {
                ScreenManager.instance.moneyDisplay.position = Vector2.LerpUnclamped(
                    new(previousMoneyPosition.x, previousMoneyPosition.y),
                    new(moneyPosition.x * screenSize.x, moneyPosition.y * screenSize.y), smoothedTimer);
            }
        }
    }
    private void OnEnable()
    {
        blockScreenPanel.SetActive(true);
        group = GetComponent<CanvasGroup>();
        if (opacity)
        {
            group.alpha = closedOpacity;
        }
        if (position)
        {
            transform.position = new(closedPosition.x * Camera.main.pixelWidth, closedPosition.y * Camera.main.pixelHeight);
        }
        if (scale)
        {
            transform.localScale = closedScale;
        }
        if (extras)
        {
            previousMoneyPosition = ScreenManager.instance.Count() != 1 ? ScreenManager.instance.moneyDisplay.position : ScreenManager.instance.moneyOriginalPosition;
            previousShowButtons = ScreenManager.instance.Count() == 1 || ScreenManager.instance.buttons[0].activeSelf;
            for (int i = 0; i < ScreenManager.instance.buttons.Length; i++)
            {
                ScreenManager.instance.buttons[i].SetActive(showButtons);
            }
        }
        open = false;
        openning = true;
        timer = 0;
    }
    public void Close()
    {
        blockScreenPanel.SetActive(true);
        openning = false;
        open = false;
        timer = Settings.Instance.animationDuration;
    }
}
