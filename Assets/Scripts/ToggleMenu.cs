using UnityEngine;

/// <summary>
/// Script to toggle menu to make entire screen visible.
/// </summary>
public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;

    public void Toggle()
    {
        menuRoot.SetActive(!menuRoot.activeSelf);
    }
}
