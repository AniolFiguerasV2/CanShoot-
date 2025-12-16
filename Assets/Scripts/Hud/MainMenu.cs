using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PointsMenu()
    {
        SceneManager.LoadScene("ScoreBoardScene");
    }
}
