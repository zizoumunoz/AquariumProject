using UnityEngine;

public class Settings
{
    private static Settings instance;
    public static Settings Instance
    {
        get
        {
            instance ??= new Settings();
            return instance;
        }
    }
    public string folder;
    public float animationDuration;
    private Settings()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        folder = "/idbfs/my_tank_game_saves";
#else
        folder = Application.persistentDataPath;
#endif

        animationDuration = 1.0f;
    }
}
