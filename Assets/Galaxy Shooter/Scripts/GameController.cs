using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Options;


public sealed class GameController : MonoBehaviour 
{
    [SerializeField]
    bool _isCoopMode = false;

    UiManager _uiManager;
    WorldManager _worldManager;
    
    bool _isGameLaunched = false;
    bool _isGamePaused = false;

    public void OnPlayerDied()
    {
        var players = FindObjectsOfType<Player>();
        if (players.Length < 2) // Because OnPlayerDied() is called before Player is destroyed
            EndGame();
    }

    public void ResumeGame()
    {
        if (NullCheck.Some(_uiManager))
            _uiManager.HidePauseMenu();

        if (NullCheck.Some(_worldManager))
            _worldManager.StartSpawning();

        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    void Start() 
    {
        _worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        if (NullCheck.Some(_uiManager))
            _uiManager.ShowMainMenu();
	}
	
	void Update() 
    {
        if (!_isGameLaunched && Input.GetKeyDown(KeyCode.Space))
        {
            _isGameLaunched = true;
            StartGame();
        }

        if (_isGameLaunched && Input.GetKeyDown(KeyCode.P))
        {
            // Pause menu
            if (_isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGameLaunched)
                EndGame();
            else
                SceneManager.LoadScene("MainMenu");
        }
	}

    void StartGame()
    {
        if (NullCheck.Some(_uiManager))
        {
            _uiManager.HideMainMenu();
            _uiManager.ClearScore();
        }

        if (NullCheck.Some(_worldManager))
        {
            if (_isCoopMode)
                _worldManager.SpawnTwoPlayers();
            else
                _worldManager.SpawnPlayer();

            _worldManager.StartSpawning();
        }
    }

    void EndGame()
    {
        if (NullCheck.Some(_uiManager))
            _uiManager.ShowMainMenu();

        if (NullCheck.Some(_worldManager))
        {
            _worldManager.StopSpawning();
            _worldManager.DestroyAll();
        }

        _isGameLaunched = false;
    }

    void PauseGame()
    {
        if (NullCheck.Some(_uiManager))
            _uiManager.ShowPauseMenu();

        if (NullCheck.Some(_worldManager))
            _worldManager.StopSpawning();

        Time.timeScale = 0f;
        _isGamePaused = true;
    }
}
