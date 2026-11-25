using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
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
        Application.Quit();
    }

    private static void ChangeScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
