using TMPro;
using UnityEngine;

/// <summary>
/// Shows the 'money' variable in a TextMeshPro text that you create in the scene.
/// Attach this to the text object (or any object) and assign the text in the Inspector.
/// </summary>
public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay Instance { get; private set; }

    /// <summary>
    /// Connect here. Example: MoneyDisplay.Instance.money += 10;  or  MoneyDisplay.Instance.AddMoney(10);
    /// </summary>
    public int money;

    [SerializeField] private TMP_Text label;
    [SerializeField] private string prefix = "$ ";

    private int shownMoney = int.MinValue;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
    }
    private void Reset()
    {
        label = GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        Instance = this;

        if (label == null) label = GetComponent<TMP_Text>();
        if (label == null)
            Debug.LogWarning("MoneyDisplay: no TextMeshPro text assigned on " + name, this);
    }

    private void Update()
    {
        if (label == null || money == shownMoney) return;

        shownMoney = money;
        label.text = prefix + money.ToString("N0");
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}