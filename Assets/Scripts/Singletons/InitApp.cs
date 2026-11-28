using UnityEngine;

public class InitApp : MonoBehaviour
{
    private static InitApp instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SaveAndLoad.Init();

        DontDestroyOnLoad(gameObject);
    }
}
