using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    [SerializeField] private Stack<GameObject> screenStack = new();

    public bool Empty()
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
        screenStack.Peek().SetActive(false);
        screenStack.Pop();
    }
    public void EmptyStack()
    {
        screenStack.Clear();
    }
    private void Awake()
    {
        instance = this;
    }
}
