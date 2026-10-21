using UnityEngine;

public class TankManager : MonoBehaviour
{
    public static TankManager instance;
    public TankDatabase database;
    [SerializeField] private GameObject tankUIPrefab;
    [SerializeField] private GameObject tankPanel;
    private void Start()
    {
        instance = this;
    }
}
