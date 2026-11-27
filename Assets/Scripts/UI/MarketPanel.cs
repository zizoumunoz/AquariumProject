using System.Collections.Generic;
using UnityEngine;

public class MarketPanel : MonoBehaviour
{
    [SerializeField] private MarketSlotUI[] slots;

    public void SetOffers(List<int> offers)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Set(offers[i], i);
        }
        MoneyManager.Instance.UpdateUI();
    }
    public void SetOffers(List<int> offers, List<int> timesBought)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Set(offers[i], i, timesBought[i]);
        }
        MoneyManager.Instance.UpdateUI();
    }
}