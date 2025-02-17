using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ChatTabsManager;

public class GameManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private TextMeshProUGUI _streamEndingTimer;
    [SerializeField] private AudioSource _newRuleSound;
    [SerializeField] private List<DifficultyLevel> _difficultyLevels = new List<DifficultyLevel>();

    private ChatTabsManager _ctm;
    private int _currentLevel;
    private int _maxLevel;
    private float _difficultyIncreaseTimer = 8;
    private float _difficultyIncreaseTime;
    private bool _gameStarted;
    private bool _gameEnded;

    [System.Serializable]
    public class DifficultyLevel
    {
        public int levelUpTimer;
        public string ViolationName;
        public CommentsSO.Violations Violation;
        public Vector2 ChatSpeed;
    }

    public delegate void GameManagerEvent();
    public static event GameManagerEvent OnGameStarted;
    public static event GameManagerEvent OnGameEnded;

    public delegate void GMNewViolationEvent(CommentsSO.Violations violation, ChatTabsManager.PunishementType punishment);
    public static event GMNewViolationEvent OnNewViolationEnforced;

    public delegate void GMNewChatSpeed(Vector2 speed);
    public static event GMNewChatSpeed OnNewChatSpeed;

    // ============== Setup ==============
    void Start()
    {
        _ctm = this.GetComponent<ChatTabsManager>();

        _maxLevel = _difficultyLevels.Count;
    }

    // ============== Function ==============
    void Update()
    {
        if (_gameEnded)
            return;

        if (_currentLevel >= _maxLevel)
            EndGame();

        int seconds = ((int)_difficultyIncreaseTimer % 60);
        int minutes = ((int)_difficultyIncreaseTimer / 60);
        string timestamp = string.Format("{0:00}:{1:00}", minutes, seconds);
        _streamEndingTimer.text = "Stream Ending in " + timestamp;

        if (_difficultyIncreaseTimer > 0)
            _difficultyIncreaseTimer -= Time.deltaTime;
        else if (_gameStarted)
            IncreaseDifficulty(_difficultyLevels[_currentLevel]);
        else
        {
            _gameStarted = true;
            OnGameStarted?.Invoke();
        }
    }

    private void IncreaseDifficulty(DifficultyLevel dlevel)
    {
        _difficultyIncreaseTime = dlevel.levelUpTimer;
        _difficultyIncreaseTimer = _difficultyIncreaseTime;
        
        if(dlevel.Violation != CommentsSO.Violations.None)
            EnforceNewRule(dlevel);
        
        OnNewChatSpeed?.Invoke(dlevel.ChatSpeed);

        _currentLevel++;

        // stream ending timer
        if(_currentLevel == _maxLevel - 1)
            _streamEndingTimer.gameObject.SetActive(true);
    }

    private void EnforceNewRule(DifficultyLevel dlevel)
    {
        ChatTabsManager.PunishementType p = GeneratePunishementType();
        _ctm.AddRule(dlevel.ViolationName, p);
        OnNewViolationEnforced?.Invoke(dlevel.Violation, p);
        _newRuleSound.Play();
    }

    private void EndGame()
    {
        Debug.Log("Game Ended!!!!!");

        _gameEnded = true;
        OnGameEnded?.Invoke();

        _streamEndingTimer.gameObject.SetActive(false);
    }

    // selects a punishemnt type from timeout, to perma ban, to make vib
    private PunishementType GeneratePunishementType()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                return PunishementType.Timeout;
            case 1:
                return PunishementType.Perma_Ban;
            case 2:
                return PunishementType.Make_VIB;
            default:
                Debug.LogWarning("Invalid int for enum conversion");
                return PunishementType.Timeout;
        }
    }
}
