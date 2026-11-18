using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
#pragma warning disable UAC1009 // Unsupported collection type for serialization
    [SerializeField] private Stack<GameObject> screenStack = new();
#pragma warning restore UAC1009 // Unsupported collection type for serialization

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

    // For screens that are turned off another way
    public void Remove(GameObject screen)
    {
        if (screenStack.Count > 0 && screenStack.Peek() == screen)
        {
            screenStack.Pop();
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
