using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static int level; //Static so that it retains it's value between scenes.

    public void LevelScreen()
    {
        SceneManager.LoadScene("Level Select"); 
    }
     
    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Play(int x)
    {
        level = x;
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
