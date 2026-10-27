using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITankController : MonoBehaviour
{
    [SerializeField] private Image tankBG;
    [SerializeField] private TextMeshProUGUI tankFishCount;
    [SerializeField] private TextMeshProUGUI tankDecorationCount;
    private Tank myTank;
    public void UpdateTank(Tank newTank)
    {
        myTank = newTank;
        tankBG.sprite = TankManager.instance.database.backgrounds[(int)newTank.type];
        tankFishCount.text = newTank.fishes.Count.ToString();
        tankDecorationCount.text = newTank.decorations.Count.ToString();
    }
    public void LoadNewTank()
    {
        TankManager.instance.LoadNewTank(myTank);
    }
}
