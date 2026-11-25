using UnityEngine;

public class ScreenSettings : MonoBehaviour
{
    private void OnEnable()
    {
        if (ScreenManager.instance.moneyDisplay != null)
        {
            ScreenManager.instance.moneyDisplay.position += (Vector3)MoneyBoxLocation;
        }
        for (int i = 0; i < ScreenManager.instance.buttons.Length; i++)
        {
            ScreenManager.instance.buttons[i].SetActive(showButtons);
        }
    }
    private void OnDisable()
    {
        if (ScreenManager.instance.moneyDisplay != null)
        {
            ScreenManager.instance.moneyDisplay.position -= (Vector3)MoneyBoxLocation;
        }
        for (int i = 0; i < ScreenManager.instance.buttons.Length; i++)
        {
            ScreenManager.instance.buttons[i].SetActive(true);
        }
    }
    public Vector2 MoneyBoxLocation;
    public bool showButtons;
}
