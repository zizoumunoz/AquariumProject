using System.Collections.Generic;
using UnityEngine;

public class DecorationMarketManager : MonoBehaviour
{
    public static DecorationMarketManager instance;
    [SerializeField] private DecorationMarketPanel panel;

    private void Awake()
    {
        instance = this;
    }

    public void TryBuy(DecorationSO deco)
    {
        if (MoneyManager.Instance.money < deco.price) return;

        DecorationSpawner.instance.Spawn(deco);
    }
}
