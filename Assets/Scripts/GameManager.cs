using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public delegate void GMNewViolationEvent(CommentsSO.Violations violation);
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
        _ctm.AddRule(dlevel.ViolationName);
        OnNewViolationEnforced?.Invoke(dlevel.Violation);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended!!!!!");

        _gameEnded = true;
        OnGameEnded?.Invoke();
    }
}
