using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public float timeToSave = 300f;
    private float saveTimer = 20f;
    public static void NewGame()
    {
        FishCollection.Clear();
        SaveAndLoad.NewData();
        ChangeScene("GameScene");
    }
    public static void LoadGame()
    {
        SaveAndLoad.LoadData(SaveAndLoad.slot);
        ChangeScene("GameScene");
    }
    public static void GoToMainMenu()
    {
        TankManager.instance.SaveTankData();
        SaveAndLoad.SaveData(TankManager.instance.database.tanks, SaveAndLoad.slot);

        ChangeScene("MainMenu");
    }

    public static void ToggleSettings(GameObject settingsGO)
    {
        settingsGO.SetActive(!settingsGO.activeSelf);
    }

    public static void ExitApp()
    {
        TankManager.instance.SaveTankData();
        SaveAndLoad.SaveData(TankManager.instance.database.tanks, SaveAndLoad.slot);

        Application.Quit();
    }

    private static void ChangeScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
    private void Update()
    {
        if (TankManager.instance == null)
        {
            return;
        }
        saveTimer -= Time.deltaTime;
        if (saveTimer < 0)
        {
            TankManager.instance.SaveTankData();
            SaveAndLoad.SaveData(TankManager.instance.database.tanks, SaveAndLoad.slot);
            saveTimer += timeToSave;
        }
    }
    private void OnApplicationQuit()
    {
        if (TankManager.instance == null)
        {
            return;
        }
        TankManager.instance.SaveTankData();
        SaveAndLoad.SaveData(TankManager.instance.database.tanks, SaveAndLoad.slot);
    }
}
