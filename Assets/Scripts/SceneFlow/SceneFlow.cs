using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public void NewGame()
    {
        ChangeScene("GameScene");
    }
    public void LoadGame()
    {
        ChangeScene("GameScene");
    }
    public void GoToMainMenu()
    {
        //TODO save game

        ChangeScene("MainMenu");
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
