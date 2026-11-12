using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTankButton : MonoBehaviour
{
    [SerializeField] private Button buyButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI tankName;
    [SerializeField] private TextMeshProUGUI tankPrice;
    [SerializeField] private Image tankSprite;
    [SerializeField] private TankDatabase database;
    private int index = 0;

    private void Start()
    {
        SetTank();
    }

    public void Left()
    {
        index--;
        if (index < 0)
        {
            index = database.tankSOs.Count - 1;
        }
        SetTank();
    }
    public void Right()
    {
        index++;
        if (index >= database.tankSOs.Count)
        {
            index = 0;
        }
        SetTank();
    }
    public void SetTank()
    {
        tankName.text = database.tankSOs[index].name;
        tankPrice.text = "$" + database.tankSOs[index].price;
        if (database.tankSOs[index].price <= MoneyManager.Instance.money)
        {
            tankPrice.color = Color.black;
            buyButton.interactable = true;
        }
        else
        {
            tankPrice.color = Color.red;
            buyButton.interactable = false;
        }
        tankSprite.sprite = database.tankSOs[index].sprite;
    }
    public void BuyTank()
    {
        database.tanks.Add(new Tank
        {
            currentAttibutes = new(database.tankSOs[index].startAttributes),
            type = database.tankSOs[index].type,
            maxFish = database.tankSOs[index].maxFish
        });
        MoneyManager.Instance.SpendMoney(database.tankSOs[index].price);
        ScreenManager.instance.EmptyStack();
        TankManager.instance.LoadNewTank(database.tanks[^1]);
    }
    private void OnEnable()
    {
        MoneyManager.Instance.moneyChanged += UpdateUI;
        SetTank();
    }
    private void OnDisable()
    {
        MoneyManager.Instance.moneyChanged -= UpdateUI;
    }
    public void UpdateUI(int money)
    {
        SetTank();
    }
}
