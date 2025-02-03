using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StreamOverlayScript : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private GameObject _startingScreen;
    [SerializeField] private TextMeshProUGUI _startingTimer;

    [SerializeField] private GameObject _endingScreen;
    [SerializeField] private TextMeshProUGUI _scoringText;

    [SerializeField] private Image _endEmoteImage;
    [SerializeField] private Sprite _topScoreSprite;
    [SerializeField] private Sprite _goodScoreSprite;
    [SerializeField] private Sprite _badScoreSprite;
    [SerializeField] private Sprite _bottomScoreSprite;

    [SerializeField] private String _topScoreString;
    [SerializeField] private String _goodScoreString;
    [SerializeField] private String _badScoreString;
    [SerializeField] private String _bottomScoreString;

    private float _countdownTimer;
    private bool _gameStarted;

    // ============== Setup ==============	
    void Start()
    {
        GameManager.OnGameStarted += HideStartingScreen;
        GameManager.OnGameEnded += ShowEnd;
        ChatManager.OnFinalScore += UpdateFinalScore;

        _countdownTimer = 8;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= HideStartingScreen;
        GameManager.OnGameEnded -= ShowEnd;
        ChatManager.OnFinalScore -= UpdateFinalScore;
    }

    // ============== Function ==============
    void Update()
    {
        if (_gameStarted)
            return;

        if (_countdownTimer > 0)
            _countdownTimer -= Time.deltaTime;

        int seconds = ((int)_countdownTimer % 60);
        _startingTimer.text = string.Format("{0:00}:{1:00}", 0, seconds);
    }

    private void HideStartingScreen()
    {
        _startingScreen.SetActive(false);
    }

    private void ShowEnd()
    {
        _endingScreen.SetActive(true);
    }

    private void UpdateFinalScore(int finalScore, int maxScore)
    {
        float scoreRatio = 100 * ((float)finalScore + (float)maxScore) / ((float)maxScore * 2);
        string s = "";
        Debug.Log("Ending score is " + finalScore + " for a ratio of " + scoreRatio);
        if (scoreRatio >= 75)
        {
            _endEmoteImage.sprite = _topScoreSprite;
            s += _topScoreString;
        } else if (scoreRatio >= 50)
        {
            _endEmoteImage.sprite = _goodScoreSprite;
            s += _goodScoreString;
        } else if (scoreRatio >= 25)
        {
            _endEmoteImage.sprite = _badScoreSprite;
            s += _badScoreString;
        } else
        {
            _endEmoteImage.sprite = _bottomScoreSprite;
            s += _bottomScoreString;
        }
        s += $" Ending Streamer Approval: {finalScore} out of {maxScore}";
        _scoringText.text = s;

    }
}
