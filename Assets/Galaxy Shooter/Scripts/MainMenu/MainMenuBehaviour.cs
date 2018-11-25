using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuBehaviour : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void LoadCoopGame()
    {
        SceneManager.LoadScene("CoopMode");
    }
}
