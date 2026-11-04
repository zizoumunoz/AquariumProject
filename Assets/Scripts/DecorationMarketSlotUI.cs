using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecorationMarketSlotUI : MonoBehaviour
{
    [SerializeField] private Image decoImage;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text descriptionLabel;
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private Button buyButton;
    [SerializeField] private DecorationSO deco;

    private void Awake()
    {
        MoneyManager.Instance.moneyChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        MoneyManager.Instance.moneyChanged -= UpdateUI;
    }

    public void Set(DecorationSO so)
    {
        deco = so;
        decoImage.sprite = so.sprite;
        nameLabel.text = so.name;
        descriptionLabel.text = so.description;
        priceLabel.text = "$" + so.price;
    }

    public void OnBuyClicked()
    {
        DecorationMarketManager.instance.TryBuy(deco);
    }

    public void UpdateUI(int money)
    {
        if (money >= deco.price)
        {
            buyButton.interactable = true;
            priceLabel.color = Color.black;
        }
        else
        {
            buyButton.interactable = false;
            priceLabel.color = Color.red;
        }
    }
}
