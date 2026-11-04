using System;
using System.Collections.Generic;
using UnityEngine;

public class DecorationMarketPanel : MonoBehaviour
{
    [SerializeField] private DecorationMarketSlotUI slotPrefab;
    [SerializeField] private List<DecorationMarketSlotUI> slotUIs;
    [SerializeField] private Transform contentRoot;

    public void SetUI(List<DecorationSO> decorations)
    {
        for (int i = 0; i < decorations.Count; i++)
        {
            if (slotUIs.Count > i)
            {
                slotUIs[i].Set(decorations[i]);
            }
            else
            {
                slotUIs.Add(Instantiate(slotPrefab, contentRoot));
                slotUIs[^1].Set(decorations[i]);
            }
        }
    }
}
