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
    [SerializeField] private List<TextChatMsg> _activeChatList = new List<TextChatMsg>();

    [Header("Chats")]
    [SerializeField] private List<CommentsSO> _commentsList = new List<CommentsSO>();
    [SerializeField] private List<UsernamesSO> _usernameList = new List<UsernamesSO>();

    private float _streamTimer;
    private bool _streamStarted;

    // For testing
    [SerializeField] private float _msgtimer;

    // ============== Setup ==============
    private void Start()
    {
        ChatTabsManager.OnModEvent += BanMsg;

        StartStream();
    }

    public void OnDestroy()
    {
        ChatTabsManager.OnModEvent -= BanMsg;
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
            _msgtimer = Random.Range(1, 3);
            SendNewChat();
            LookForPastMsgs();
        }
    }

    private void SendNewChat()
    {
        int randComment = Random.Range(0, _commentsList.Count);
        int randUsername = Random.Range(0, _usernameList.Count);

        DisplayChatMsg(_usernameList[randUsername].GetUsername(), _commentsList[randComment].Message);
    }

    private void LookForPastMsgs()
    {
        if (_activeChatList.Count == 0)
            return;
        
        if (_activeChatList[0].GetTimeAlive() < 1f)
            return;

        if (!RectContainsAnother(_maskRect, _activeChatList[0].GetComponent<RectTransform>()))
        {
            Debug.Log($"Message went out of bounds! deleting", _activeChatList[0].gameObject);
            DeleteOldMsg(_activeChatList[0]);
        }
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

        Debug.Log($"topLeftCornerIn: {topLeftCornerIn} {rctCorners[1].y} >= {msgCorners[1].y}" +
            $"\n botRightCornerIn: {botRightCornerIn} {rctCorners[1].y} >= {msgCorners[3].y}");

        return topLeftCornerIn || botRightCornerIn;
    }

    private void DisplayChatMsg(string chatName, string chatMsg)
    {
        int seconds = ((int)_streamTimer % 60);
        int minutes = ((int)_streamTimer / 60);
        string timestamp = string.Format("{0:00}:{1:00}", minutes, seconds);

        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.Setup(timestamp, chatName, chatMsg, this);

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);
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
        
        _activeChatList.Remove(chatMsg);

        chatMsg.DestroyMsg();
    }
}
