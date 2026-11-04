using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public void NewGame()
    {
        //TODO delete save
        // CHANGE BACK TO ORIGINAL BEFORE MERGE
        //ChangeScene("GameScene");
        ChangeScene("Z_GameScene");
    }
    public void LoadGame()
    {
        //TODO load save

        //ChangeScene("GameScene");
        ChangeScene("Z_GameScene");
    }
    public void GoToMainMenu()
    {
        //TODO save game

        ChangeScene("Z_MainMenu");
        //ChangeScene("MainMenu");
    }

    public void ToggleSettings(GameObject settingsGO)
    {
        settingsGO.SetActive(!settingsGO.activeSelf);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    private void ChangeScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
