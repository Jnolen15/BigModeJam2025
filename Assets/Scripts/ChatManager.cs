using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private GameObject _chatMsgPref;
    [SerializeField] private RectTransform _chatArea;

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
