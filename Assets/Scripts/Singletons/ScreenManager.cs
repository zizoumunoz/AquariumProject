using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
#pragma warning disable UAC1009 // Unsupported collection type for serialization
    [SerializeField] private Stack<GameObject> screenStack = new();
#pragma warning restore UAC1009 // Unsupported collection type for serialization

    public RectTransform moneyDisplay;
    public GameObject[] buttons = new GameObject[0];

    public bool IsEmpty()
    {
        return screenStack.Count == 0;
    }

    public void OpenScreen(GameObject screen)
    {
        screenStack.Push(screen);
        screenStack.Peek().SetActive(true);
    }

    public void PopScreen()
    {
        if (screenStack.Count == 0)
        {
            return;
        }

        // pop first then deactivate
        GameObject top = screenStack.Pop();
        top.SetActive(false);

        if (screenStack.Count > 0)
        {
            screenStack.Peek().SetActive(true);
        }
    }

    public void EmptyStack()
    {
        while (screenStack.Count > 0)
        {
            screenStack.Peek().SetActive(false);
            screenStack.Pop();
        }
    }

    private void Awake()
    {
        instance = this;
    }
}