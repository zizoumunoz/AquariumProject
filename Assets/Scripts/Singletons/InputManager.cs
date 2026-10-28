using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private BoolInput[] bools;
    [SerializeField] private Vector2Input[] vector2s;

    public BoolInput GetBoolInput(string actionName)
    {
        foreach (BoolInput boolInput in bools)
        {
            Debug.Log(boolInput.actionName);
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