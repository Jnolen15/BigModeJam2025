using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CommentsSO;

public class ChatManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [Header("Refrences")]
    [SerializeField] private GameObject _chatMsgPref;
    [SerializeField] private RectTransform _chatArea;
    [SerializeField] public RectTransform _maskRect;
    [SerializeField] public Vector2 _chatSpeed;
    [SerializeField] public int _maxScore;

    [Header("Chats")]
    [SerializeField] private List<UsernamesSO> _usernameList = new List<UsernamesSO>();
    [SerializeField] private List<CommentsSO> _generalMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _profanityMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _linksMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _promotingMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _capsMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _meanMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _over30MSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _streamOverMSGList = new List<CommentsSO>();
    private List<List<CommentsSO>> _violatingMessageCattegories = new List<List<CommentsSO>>();

    private int _currentScore;
    private float _msgtimer;
    private float _streamTimer;
    private bool _streamStarted;
    private bool _gameEnded;
    private List<TextChatMsg> _activeChatList = new List<TextChatMsg>();
    [SerializeField] private List<CommentsSO.Violations> _violationsList = new List<CommentsSO.Violations>();
    [SerializeField] private List<ChatTabsManager.PunishementType> _punishmentsList = new List<ChatTabsManager.PunishementType>();

    public delegate void ChatManagerEvent(CommentsSO.Violations violation, ChatTabsManager.PunishementType punishment, TextChatMsg chatMsg);
    public static event ChatManagerEvent OnMissedMessage;
    public static event ChatManagerEvent OnFalseBan;

    public delegate void ChatManagerScore(int score, int max);
    public static event ChatManagerScore OnScoreUpdated;
    public static event ChatManagerScore OnFinalScore;

    // ============== Setup ==============
    private void Start()
    {
        ChatTabsManager.OnModEvent += BanMsg;
        GameManager.OnNewViolationEnforced += NewViolation;
        GameManager.OnNewChatSpeed += NewChatSpeed;
        GameManager.OnGameStarted += StartStream;
        GameManager.OnGameEnded += EndStream;
    }

    public void OnDestroy()
    {
        ChatTabsManager.OnModEvent -= BanMsg;
        GameManager.OnNewViolationEnforced -= NewViolation;
        GameManager.OnNewChatSpeed -= NewChatSpeed;
        GameManager.OnGameStarted -= StartStream;
        GameManager.OnGameEnded -= EndStream;
    }

    public void StartStream()
    {
        _streamStarted = true;
    }

    public void EndStream()
    {
        _gameEnded = true;
        OnFinalScore?.Invoke(_currentScore, _maxScore);
    }

    // ============== Function ==============
    void Update()
    {
        if (!_streamStarted)
            return;

        _streamTimer += Time.deltaTime;

        if (_msgtimer > 0)
            _msgtimer -= Time.deltaTime;
        else
        {
            _msgtimer = Random.Range(_chatSpeed.x, _chatSpeed.y);
            
            if(_gameEnded)
                SendNewGameOverChat();
            else
                SendNewChat();

            LookForPastMsgs();
        }
    }

    private void SendNewChat()
    {
        int randUsername = Random.Range(0, _usernameList.Count);

        int rand = Random.Range(0, 100);
        if (rand < 90)
            DisplayChatMsg(_usernameList[randUsername], _generalMSGList[Random.Range(0, _generalMSGList.Count)]);
        else
            SendNewViolatingChat(randUsername);
    }

    private void SendNewViolatingChat(int randUsername)
    {
        int rand = Random.Range(0, _violatingMessageCattegories.Count);
        CommentsSO comment = _violatingMessageCattegories[rand][Random.Range(0, _violatingMessageCattegories[rand].Count)];

        DisplayChatMsg(_usernameList[randUsername], comment);
    }

    private void SendNewGameOverChat()
    {
        int randUsername = Random.Range(0, _usernameList.Count);
        DisplayChatMsg(_usernameList[randUsername], _streamOverMSGList[Random.Range(0, _streamOverMSGList.Count)]);
    }

    private void LookForPastMsgs()
    {
        if (_activeChatList.Count == 0)
            return;
        
        if (_activeChatList[0].GetTimeAlive() < 0.5f)
            return;

        if (!RectContainsAnother(_maskRect, _activeChatList[0].GetComponent<RectTransform>()))
            TestMessageForViolations(_activeChatList[0]);
    }

    public static bool RectContainsAnother(RectTransform first, RectTransform second)
    {
        Vector3[] rctCorners = new Vector3[4];
        first.GetWorldCorners(rctCorners);

        Vector3[] msgCorners = new Vector3[4];
        second.GetWorldCorners(msgCorners);

        bool topLeftCornerIn = false;
        bool botRightCornerIn = false;

        if (rctCorners[1].y >= msgCorners[1].y)
            topLeftCornerIn = true;

        if (rctCorners[1].y >= msgCorners[3].y)
            botRightCornerIn = true;

        //Debug.Log($"topLeftCornerIn: {topLeftCornerIn} {rctCorners[1].y} >= {msgCorners[1].y}" +
        //    $"\n botRightCornerIn: {botRightCornerIn} {rctCorners[1].y} >= {msgCorners[3].y}");

        return topLeftCornerIn || botRightCornerIn;
    }

    private void DisplayChatMsg(UsernamesSO username, CommentsSO comment)
    {
        int seconds = ((int)_streamTimer % 60);
        int minutes = ((int)_streamTimer / 60);
        string timestamp = string.Format("{0:00}:{1:00}", minutes, seconds);

        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.Setup(username, comment, timestamp, this);

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);
    }
    
    private void DisplayModMsg(string chatMsg)
    {
        int seconds = ((int)_streamTimer % 60);
        int minutes = ((int)_streamTimer / 60);
        string timestamp = string.Format("{0:00}:{1:00}", minutes, seconds);

        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.SetupModMsg(timestamp, chatMsg, this);

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);

        LookForPastMsgs();
    }

    private void NewViolation(CommentsSO.Violations newViolation, ChatTabsManager.PunishementType punishment)
    {
        _violationsList.Add(newViolation);
        _punishmentsList.Add(punishment);

        switch (newViolation)
        {
            case CommentsSO.Violations.AllCaps:
                _violatingMessageCattegories.Add(_capsMSGList);
                break;
            case CommentsSO.Violations.Links:
                _violatingMessageCattegories.Add(_linksMSGList);
                break;
            case CommentsSO.Violations.Mean:
                _violatingMessageCattegories.Add(_meanMSGList);
                break;
            case CommentsSO.Violations.MessagesOver30:
                _violatingMessageCattegories.Add(_over30MSGList);
                break;
            case CommentsSO.Violations.Profanity:
                _violatingMessageCattegories.Add(_profanityMSGList);
                break;
            case CommentsSO.Violations.Promoting:
                _violatingMessageCattegories.Add(_promotingMSGList);
                break;
            default:
                return;
        }
        Debug.Log("Added new rule and new message list: " + newViolation);
    }

    private void NewChatSpeed(Vector2 newSpeed)
    {
        _chatSpeed = newSpeed;
    }

    private void TestMessageForViolations(TextChatMsg chatMsg)
    {
        // check message (make sure its not a mod message or one that was deleted)
        if (!chatMsg.GetNotClickable())
        {
            CommentsSO comment = chatMsg.GetComment();
            CommentsSO.Violations violation = comment.violation;

            if (violation != CommentsSO.Violations.None && _violationsList.Contains(violation))
            {
                Debug.Log("Violating message was let past!! \n " + comment.Message);

                // Send violation message to reports area
                OnMissedMessage?.Invoke(violation, ChatTabsManager.PunishementType.Temp_Ban, chatMsg); // punishment type is not used here just needs to be passed

                AdjustScore(-5);
            }
        }

        DeleteOldMsg(chatMsg);
    }

    public void DeleteOldMsg(TextChatMsg chatMsg)
    {
        if (!_activeChatList.Contains(chatMsg))
        {
            Debug.LogWarning("Chat message not found in _activeChatList");
            return;
        }

        _activeChatList.Remove(chatMsg);

        chatMsg.DestroyMsg();
    }

    public void BanMsg(ChatTabsManager.PunishementType p, TextChatMsg chatMsg)
    {
        // checking to see if chat is actionable
        if (!_activeChatList.Contains(chatMsg))
        {
            Debug.LogWarning("Chat message not found in _activeChatList");
            return;
        }

        int index = _violationsList.IndexOf(chatMsg.GetComment().violation); // returns -1 if item was not found
        if (index == -1)
        {
            // not a violation
            AdjustScore(-5);
            OnFalseBan?.Invoke(chatMsg.GetComment().violation, p, chatMsg);
        }
        else
        {
            // was a recognized violation
            if (p == _punishmentsList[index])
            {
                // correct punishment
                AdjustScore(+5);
            }
            else
            {
                // incorrect punishment
                AdjustScore(-3);
                OnFalseBan?.Invoke(chatMsg.GetComment().violation, p, chatMsg);
            }
        }

        // something about punishment here?
        /*
        if (p == ChatTabsManager.PunishementType.Make_Buddy || p == ChatTabsManager.PunishementType.Make_NonBuddy || p == ChatTabsManager.PunishementType.Make_VIB)
            return;



        // check if violated rules
        CommentsSO comment = chatMsg.GetComment();
        CommentsSO.Violations violation = comment.violation;
        if (violation != CommentsSO.Violations.None && _violationsList.Contains(violation))
        {
            AdjustScore(+5);
        }
        else
        {
            // false ban
            AdjustScore(-5);
            OnFaseBan?.Invoke(violation, chatMsg);
        }
        s
        */

        chatMsg.StikeOutMsg();

        DisplayModMsg(p.ToString());
    }

    private void AdjustScore(int adjustment)
    {
        _currentScore += adjustment;

        if (_currentScore > _maxScore)
            _currentScore = _maxScore;
        else if (_currentScore < -_maxScore)
            _currentScore = -_maxScore;

        //Debug.Log($"Score changed by {adjustment} now {_currentScore}");

        OnScoreUpdated?.Invoke(_currentScore, _maxScore);
    }
}
