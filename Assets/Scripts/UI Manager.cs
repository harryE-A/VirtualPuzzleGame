using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private void Level()
    {
        SceneManager.LoadScene("Level Select");
    }

    private void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    private void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
