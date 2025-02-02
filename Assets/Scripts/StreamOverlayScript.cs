using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreamOverlayScript : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private GameObject _startingScreen;
    [SerializeField] private TextMeshProUGUI _startingTimer;

    private float _countdownTimer;
    private bool _gameStarted;

    // ============== Setup ==============	
    void Start()
    {
        GameManager.OnGameStarted += HideStartingScreen;

        _countdownTimer = 8;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= HideStartingScreen;
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
}
