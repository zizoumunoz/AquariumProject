using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketPanel : MonoBehaviour
{
    [SerializeField] private MarketSlotUI[] slots;

    private void OnEnable()
    {
        MarketManager.instance.OpenMarket();
    }

    public void SetOffers(List<FishSO> offers)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Set(i < offers.Count ? offers[i] : null);
        }
    }
}