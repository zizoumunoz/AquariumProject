using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private BoolInput[] bools;
    [SerializeField] private Vector2Input[] vector2s;
    private BoolInput click;
    private Vector2Input mousePos;
    private Camera cam;
    private FishController lastFish;
    private Color highColor = new(0, 0, 0, 0.4f);
    private Color noHighColor = new(0, 0, 0, 0);
    public BoolInput GetBoolInput(string actionName)
    {
        foreach (BoolInput boolInput in bools)
        {
            if (actionName == boolInput.actionName)
            {
                return boolInput;
            }
        }
        throw new Exception($"Action with action name {actionName} doesnt exist");
    }
    public Vector2Input GetVector2Input(string actionName)
    {
        foreach (Vector2Input vector in vector2s)
        {
            if (actionName == vector.actionName)
            {
                return vector;
            }
        }
        throw new Exception($"Action with action name {actionName} doesnt exist");
    }
    private void Awake()
    {
        Instance = this;
        foreach (BoolInput input in bools)
        {
            input.SetUp(inputActions);
        }
        foreach (Vector2Input input in vector2s)
        {
            input.SetUp(inputActions);
        }

        click = GetBoolInput("LeftClick");
        mousePos = GetVector2Input("MousePosition");
        cam = Camera.main;
    }
    private void Update()
    {
        foreach (BoolInput input in bools)
        {
            input.Update();
        }
        foreach (Vector2Input input in vector2s)
        {
            input.Update();
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (lastFish != null)
            {
                lastFish.highlight.color = noHighColor;
                lastFish = null;
            }
            return;
        }

        Vector3 pos = cam.ScreenToWorldPoint(mousePos.value);
        pos.z = 0f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.2f, LayerMask.GetMask("Fish"));
        if (hits.Length > 0)
        {
            if (lastFish != null)
            {
                if (lastFish != hits[0].GetComponent<FishController>())
                {
                    lastFish.highlight.color = noHighColor;
                    lastFish = hits[0].GetComponent<FishController>();
                    lastFish.highlight.color = highColor;
                }
            }
            else
            {
                lastFish = hits[0].GetComponent<FishController>();
                lastFish.highlight.color = highColor;
            }
        }
        else
        {
            if (lastFish != null)
            {
                lastFish.highlight.color = noHighColor;
                lastFish = null;
            }
        }

        if (click.pressed)
        {
            OnClick(hits);
        }
    }
    public void OnClick(Collider2D[] hits)
    {
        if (lastFish != null)
        {
            if (hits.Length > 0)
            {
                FishPanel.instance.SetUp(lastFish);
                return;
            }
            else
            {
                ScreenManager.instance.EmptyStack();
                return;
            }
        }

        if (ScreenManager.instance.IsEmpty())
        {
            FoodSpawner.instance.SpawnFood();
        }
        else
        {
            ScreenManager.instance.EmptyStack();
        }
        return;
    }
}

[Serializable]
public abstract class CustomInput
{
    [SerializeField] protected InputAction action;
    public string actionName;
    public void SetUp(InputActionAsset inputActions)
    {
        action = inputActions.FindAction(actionName);
    }
    public abstract void Update();
}

[Serializable]
public class BoolInput : CustomInput
{
    public bool pressed;
    public bool released;
    public bool held;
    public override void Update()
    {
        pressed = !held && action.IsPressed();
        released = held && !action.IsPressed();
        held = action.IsPressed();
    }
}

[Serializable]
public class Vector2Input : CustomInput
{
    public Vector2 value;
    public override void Update()
    {
        value = action.ReadValue<Vector2>();
    }
}