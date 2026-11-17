using UnityEngine;

public class SellingManager : MonoBehaviour
{
    public static void SellFish(FishPanel fishPanel)
    {
        FishController fish = fishPanel.fish;
        MoneyManager.Instance.AddMoney(fish.Price);
        ScreenManager.instance.EmptyStack();

        Destroy(fish.gameObject);
    }
}
