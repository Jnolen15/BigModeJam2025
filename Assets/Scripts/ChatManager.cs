using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [Header("Refrences")]
    [SerializeField] private GameObject _chatMsgPref;
    [SerializeField] private RectTransform _chatArea;
    [SerializeField] public RectTransform _maskRect;
    [SerializeField] public Vector2 _chatSpeed;

    [Header("Chats")]
    [SerializeField] private List<UsernamesSO> _usernameList = new List<UsernamesSO>();
    [SerializeField] private List<CommentsSO> _generalMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _profanityMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _linksMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _promotingMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _capsMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _meanMSGList = new List<CommentsSO>();
    [SerializeField] private List<CommentsSO> _over30MSGList = new List<CommentsSO>();
    private List<List<CommentsSO>> _violatingMessageCattegories = new List<List<CommentsSO>>();

    private float _msgtimer;
    private float _streamTimer;
    private bool _streamStarted;
    private List<TextChatMsg> _activeChatList = new List<TextChatMsg>();
    [SerializeField] private List<CommentsSO.Violations> _violationsList = new List<CommentsSO.Violations>();

    public delegate void ChatManagerEvent(CommentsSO.Violations violation, TextChatMsg chatMsg);
    public static event ChatManagerEvent OnMissedMessage;

    // ============== Setup ==============
    private void Start()
    {
        ChatTabsManager.OnModEvent += BanMsg;
        GameManager.OnNewViolationEnforced += NewViolation;

        StartStream();
    }

    public void OnDestroy()
    {
        ChatTabsManager.OnModEvent -= BanMsg;
        GameManager.OnNewViolationEnforced -= NewViolation;
    }

    public void StartStream()
    {
        _streamStarted = true;
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

    private void NewViolation(CommentsSO.Violations newViolation)
    {
        _violationsList.Add(newViolation);

        switch (newViolation)
        {
            case CommentsSO.Violations.NoAllCaps:
                _violatingMessageCattegories.Add(_capsMSGList);
                break;
            case CommentsSO.Violations.NoLinks:
                _violatingMessageCattegories.Add(_linksMSGList);
                break;
            case CommentsSO.Violations.NoMean:
                _violatingMessageCattegories.Add(_meanMSGList);
                break;
            case CommentsSO.Violations.NoMessagesOver30:
                _violatingMessageCattegories.Add(_over30MSGList);
                break;
            case CommentsSO.Violations.NoProfanity:
                _violatingMessageCattegories.Add(_profanityMSGList);
                break;
            case CommentsSO.Violations.NoPromoting:
                _violatingMessageCattegories.Add(_promotingMSGList);
                break;
            default:
                return;
        }

        Debug.Log("Added new rule and new message list: " + newViolation);
    }

    private void TestMessageForViolations(TextChatMsg chatMsg)
    {
        // check message (make sure its not a mod message or one that was deleted)
        if (!chatMsg.GetNotClickable())
        {
            CommentsSO comment = chatMsg.GetComment();
            CommentsSO.Violations violation = comment.violation;

            // Only testing 1 violation for now
            if (violation != CommentsSO.Violations.None && _violationsList.Contains(violation))
            {
                Debug.Log("Violating message was let past!! \n " + comment.Message);

                // Send violation message to reports area
                OnMissedMessage?.Invoke(violation, chatMsg);
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
        // something about punishment here?
        if (p == ChatTabsManager.PunishementType.Make_Buddy || p == ChatTabsManager.PunishementType.Make_NonBuddy || p == ChatTabsManager.PunishementType.Make_VIB)
            return;
        
        if (!_activeChatList.Contains(chatMsg))
        {
            Debug.LogWarning("Chat message not found in _activeChatList");
            return;
        }

        chatMsg.StikeOutMsg();

        DisplayModMsg(p.ToString());
    }
}
