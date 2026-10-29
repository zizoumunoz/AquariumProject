using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketPanel : MonoBehaviour
{
    [SerializeField] private MarketSlotUI[] slots;

    public event Action<FishSO> FishBought;

    private void Awake()
    {
        foreach (MarketSlotUI slot in slots)
            slot.Bought += OnSlotBought;
    }

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

    private void OnSlotBought(FishSO fish)
    {
        if (FishBought != null) FishBought(fish);
    }
}