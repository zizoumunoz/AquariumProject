using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] public int money = 0;
    [SerializeField] private int initialMoney = 100;
    public delegate void MoneyChanged(int money);
    public MoneyChanged moneyChanged;
    private bool moneyGiven = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!moneyGiven)
        {
            AddMoney(initialMoney);
            moneyGiven = true;
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyChanged(money);
    }
    public void SpendMoney(int amount)
    {
        money -= amount;
        moneyChanged(money);
    }
}