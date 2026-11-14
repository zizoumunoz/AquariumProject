using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private string prefix = "$";
    private void OnEnable()
    {
        MoneyManager.Instance.moneyChanged += UpdateUI;
    }
    private void OnDisable()
    {
        MoneyManager.Instance.moneyChanged -= UpdateUI;
    }
    private void UpdateUI(int money)
    {
        label.text = prefix + money.ToString();
    }
}
