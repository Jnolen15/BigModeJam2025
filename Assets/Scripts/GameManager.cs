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
    [SerializeField] private float _difficultyIncreaseTimer;
    private float _difficultyIncreaseTime;

    [System.Serializable]
    public class DifficultyLevel
    {
        public int levelUpTimer;
        public string ViolationName;
        public CommentsSO.Violations Violation;
        public Vector2 ChatSpeed;
    }

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
        if (_currentLevel >= _maxLevel)
            return;

        if (_difficultyIncreaseTimer > 0)
            _difficultyIncreaseTimer -= Time.deltaTime;
        else
            IncreaseDifficulty(_difficultyLevels[_currentLevel]);
    }

    private void IncreaseDifficulty(DifficultyLevel dlevel)
    {
        _difficultyIncreaseTime = dlevel.levelUpTimer;
        _difficultyIncreaseTimer = _difficultyIncreaseTime;
        EnforceNewRule(dlevel);
        OnNewChatSpeed?.Invoke(dlevel.ChatSpeed);
    }

    private void EnforceNewRule(DifficultyLevel dlevel)
    {
        _ctm.AddRule(dlevel.ViolationName);
        OnNewViolationEnforced?.Invoke(dlevel.Violation);
        _currentLevel++;
    }
}
