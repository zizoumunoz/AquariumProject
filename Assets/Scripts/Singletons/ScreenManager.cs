using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
#pragma warning disable UAC1009 // Unsupported collection type for serialization
    [SerializeField] private Stack<PanelAnimator> screenStack = new();
#pragma warning restore UAC1009 // Unsupported collection type for serialization
    public Vector3 moneyOriginalPosition;
    public RectTransform moneyDisplay;
    public GameObject[] buttons = new GameObject[0];

    public bool IsEmpty()
    {
        return screenStack.Count == 0;
    }
    public int Count()
    {
        return screenStack.Count;
    }

    public void OpenScreen(PanelAnimator screen)
    {
        screenStack.Push(screen);
        screenStack.Peek().gameObject.SetActive(true);
    }

    public void PopScreen()
    {
        if (screenStack.Count == 0)
        {
            return;
        }

        // pop first then deactivate
        PanelAnimator top = screenStack.Pop();
        top.Close();

        if (screenStack.Count > 0)
        {
            screenStack.Peek().gameObject.SetActive(true);
        }
    }

    public void EmptyStack()
    {
        while (screenStack.Count > 1)
        {
            screenStack.Peek().gameObject.SetActive(false);
            screenStack.Pop();
        }
        if (screenStack.Count > 0)
        {
            screenStack.Peek().Close();
            screenStack.Pop();
        }
    }

    private void Awake()
    {
        if (moneyDisplay)
        {
            moneyOriginalPosition = moneyDisplay.position;
        }
        instance = this;
    }
}