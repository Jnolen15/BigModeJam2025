using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private GameObject _chatMsgPref;
    [SerializeField] private RectTransform _chatArea;
    [SerializeField] public RectTransform _maskRect;
    [SerializeField] private List<TextChatMsg> _activeChatList = new List<TextChatMsg>();

    // For testing
    [SerializeField] private float _msgtimer;

    // ============== Setup ==============
    void Start()
    {

    }

    // ============== Function ==============
    void Update()
    {
        if (_msgtimer > 0)
            _msgtimer -= Time.deltaTime;
        else
        {
            _msgtimer = Random.Range(1, 3);
            DisplayChatMsg("Keenan", "Ban me");
        }

        LookForPastMsgs();
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
            BanMe(_activeChatList[0]);
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
        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.Setup("00:00", chatName, chatMsg, this);

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);
    }

    public void BanMe(TextChatMsg chatMsg)
    {
        if (!_activeChatList.Contains(chatMsg))
        {
            Debug.LogWarning("Chat message not found in _activeChatList");
            return;
        }
        
        _activeChatList.Remove(chatMsg);

        chatMsg.DestroyMsg();
    }
}
