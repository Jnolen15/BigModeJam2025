using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private List<string> _rulesList = new List<string>();
    [SerializeField] private List<CommentsSO.Violations> _violationsList = new List<CommentsSO.Violations>();
    [SerializeField] private int _difficultyIncreaseTime;

    private ChatTabsManager _ctm;
    private int _currentLevel;
    private int _maxLevel;
    private float _difficultyIncreaseTimer;

    public delegate void GMNewViolationEvent(CommentsSO.Violations violation);
    public static event GMNewViolationEvent OnNewViolationEnforced;

    // ============== Setup ==============	
    void Start()
    {
        _ctm = this.GetComponent<ChatTabsManager>();

        _maxLevel = _violationsList.Count;
    }

    // ============== Function ==============
    void Update()
    {
        if (_currentLevel >= _maxLevel)
            return;

        if (_difficultyIncreaseTimer > 0)
            _difficultyIncreaseTimer -= Time.deltaTime;
        else
        {
            _difficultyIncreaseTimer = _difficultyIncreaseTime;
            EnforceNewRule();
        }
    }

    private void EnforceNewRule()
    {
        _ctm.AddRule(_rulesList[_currentLevel]);
        OnNewViolationEnforced?.Invoke(_violationsList[_currentLevel]);
        _currentLevel++;
    }
}
