using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static int level;

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

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

}
