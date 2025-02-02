using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportsManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [Header("Refrences")]
    [SerializeField] private GameObject _chatMsgPref;
    [SerializeField] private RectTransform _chatArea;
    [SerializeField] public RectTransform _maskRect;

    private ChatTabsManager _ctm;

    private List<TextChatMsg> _activeChatList = new List<TextChatMsg>();

    // ============== Setup ==============
    private void Start()
    {
        _ctm = this.GetComponentInParent<ChatTabsManager>();

        ChatManager.OnMissedMessage += DisplayReportMsg;
        ChatManager.OnFaseBan += DisplayFalseBanMsg;
    }

    public void OnDestroy()
    {
        ChatManager.OnMissedMessage -= DisplayReportMsg;
        ChatManager.OnFaseBan += DisplayFalseBanMsg;
    }

    // ============== Function ==============
    private void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);
    }

    public void DisplayReportMsg(CommentsSO.Violations violation, TextChatMsg chatMsg)
    {
        string msgText = $"<color=red>Violated rule {violation}: </color>" + chatMsg.GetMessageText();

        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.SetupReportMsg(chatMsg.GetUsername(), msgText, chatMsg.GetBuddyStatus(), chatMsg.GetTimeStamp());

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);

        _ctm.ShowNotificationOnReportTab();

        LookForPastMsgs();
    }

    public void DisplayFalseBanMsg(CommentsSO.Violations violation, TextChatMsg chatMsg)
    {
        string msgText = $"<color=red>False ban: </color>" + chatMsg.GetMessageText();

        TextChatMsg msg = Instantiate(_chatMsgPref, _chatArea).GetComponentInChildren<TextChatMsg>();
        msg.SetupReportMsg(chatMsg.GetUsername(), msgText, chatMsg.GetBuddyStatus(), chatMsg.GetTimeStamp());

        _activeChatList.Add(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatArea);

        _ctm.ShowNotificationOnReportTab();

        LookForPastMsgs();
    }

    private void LookForPastMsgs()
    {
        if (_activeChatList.Count == 0)
            return;

        if (_activeChatList[0].GetTimeAlive() < 1f)
            return;

        if (!RectContainsAnother(_maskRect, _activeChatList[0].GetComponent<RectTransform>()))
            DeleteOldMsg(_activeChatList[0]);
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
}
