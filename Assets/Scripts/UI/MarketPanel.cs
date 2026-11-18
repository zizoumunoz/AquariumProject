using System.Collections.Generic;
using UnityEngine;

public class MarketPanel : MonoBehaviour
{
    [SerializeField] private MarketSlotUI[] slots;

    public void SetOffers(List<FishSO> offers)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Set(i < offers.Count ? offers[i] : null, i);
        }
        MoneyManager.Instance.UpdateUI();
    }
    public void SetOffers(List<FishSO> offers, List<int> timesBought)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Set(i < offers.Count ? offers[i] : null, i, timesBought[i]);
        }
        MoneyManager.Instance.UpdateUI();
    }
}