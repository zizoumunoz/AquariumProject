using UnityEngine;

public class SellingManager : MonoBehaviour
{
    public static void SellFish(FishPanel fishPanel)
    {
        FishStats fish = fishPanel.fishStats;
        MoneyManager.Instance.AddMoney(fish.price);
        ScreenManager.instance.EmptyStack();

        Destroy(fish.gameObject);
    }
}
