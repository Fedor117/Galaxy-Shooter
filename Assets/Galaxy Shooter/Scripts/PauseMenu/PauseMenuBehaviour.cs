using UnityEngine;
using UnityEngine.SceneManagement;
using Options;

public sealed class PauseMenuBehaviour : MonoBehaviour
{
    public void ResumeGame()
    {
        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (NullCheck.Some(gameController))
            gameController.ResumeGame();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}