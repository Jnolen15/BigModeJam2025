using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChatTabsManager;

public class GameManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
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
    }

    private void EnforceNewRule(DifficultyLevel dlevel)
    {
        ChatTabsManager.PunishementType p = GeneratePunishementType();
        _ctm.AddRule(dlevel.ViolationName, p);
        OnNewViolationEnforced?.Invoke(dlevel.Violation, p);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended!!!!!");

        _gameEnded = true;
        OnGameEnded?.Invoke();
    }

    // selects a punishemnt type from timeout, to perma ban, to make vib
    private PunishementType GeneratePunishementType()
    {
        while (true)
        {
            switch (UnityEngine.Random.Range(0, 6))
            {
                case 0:
                    return PunishementType.Timeout;
                case 1:
                    break; // unused punishments just loop for now
                case 2:
                    return PunishementType.Perma_Ban;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    return PunishementType.Make_VIB;

                default:
                    Debug.LogWarning("Invalid int for enum conversion");
                    return PunishementType.Timeout;
            }
        }
    }
}
