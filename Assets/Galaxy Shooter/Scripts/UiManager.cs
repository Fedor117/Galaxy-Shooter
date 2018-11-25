using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Options;

public sealed class UiManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] _livesImages;

    [SerializeField]
    Image _displayedLivesImage;

    [SerializeField]
    Text _scoreTextView;

    [SerializeField]
    Text _highScoreTextView;

    [SerializeField]
    Image _mainMenuImage;

    [SerializeField]
    GameObject _pauseMenuPanel;

    Animator _pauseAnimator;

    int _score;
    int _highScore;

    public void UpdateLives(int currentLives)
    {
        _displayedLivesImage.sprite = _livesImages[currentLives];
    }

    public void UpdateScore(int score)
    {
        _score += score;
        _scoreTextView.text = "Score: " + _score;

        if (_highScore < _score)
        {
            _highScore = _score;
            UpdateHighScore();
        }
    }

    public void ClearScore()
    {
        _score = 0;
        UpdateScore(_score);
    }

    public void UpdateHighScore()
    {
        _highScoreTextView.text = "High Score: " + _highScore;
        PlayerPrefs.SetInt("HighScore", _highScore);
    }

    public void ShowMainMenu()
    {
        HidePauseMenu();

        _mainMenuImage.enabled = true;

        if (NullCheck.Some(_displayedLivesImage))
            _displayedLivesImage.enabled = false;
    }

    public void HideMainMenu()
    {
        _mainMenuImage.enabled = false;

        if (NullCheck.Some(_displayedLivesImage))
            _displayedLivesImage.enabled = true;
    }

    public void ShowPauseMenu()
    {
        _pauseMenuPanel.SetActive(true);

        if (NullCheck.Some(_pauseAnimator))
        {
            _pauseAnimator.SetBool("isPaused", true);
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        if (NullCheck.Some(_displayedLivesImage))
            _displayedLivesImage.enabled = false;
    }

    public void HidePauseMenu()
    {
        _pauseMenuPanel.SetActive(false);

        if (NullCheck.Some(_displayedLivesImage))
            _displayedLivesImage.enabled = true;
    }

    void Start()
    {
        _pauseAnimator = _pauseMenuPanel.GetComponent<Animator>();
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScore();
    }
}
