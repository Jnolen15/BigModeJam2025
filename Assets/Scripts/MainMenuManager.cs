using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string MenuSceneName;
    [SerializeField] private string GameSceneName;

    public void LoadGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
