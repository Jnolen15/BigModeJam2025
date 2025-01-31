using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChatMsg : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private TextMeshProUGUI _timeStamp;
    [SerializeField] private TextMeshProUGUI _chatterName;
    [SerializeField] private TextMeshProUGUI _message;
    
    private float _aliveTime;
    private ChatManager _chatMan;
    private bool _banned;

    public delegate void ChatEvent(ChatTabsManager.BuddyStatus b, string name, string timeStamp, string message, TextChatMsg chatMsg);
    public static event ChatEvent OnChatClicked;

    // ============== Setup ==============	
    public void Setup(string timeStamp, string chatterName, Color chatterColor, string msg, ChatManager chatMngr)
    {
        _timeStamp.text = timeStamp;
        _chatterName.text = chatterName + ":";
        _chatterName.color = chatterColor;
        _message.text = msg;
        _chatMan = chatMngr;

        // Something here for buddy badge
    }

    // ============== Function ==============
    private void Update()
    {
        _aliveTime += Time.deltaTime;
    }

    public void OnClick()
    {
        if (_banned)
            return;

        OnChatClicked?.Invoke(ChatTabsManager.BuddyStatus.Non_Buddy, _chatterName.text, _timeStamp.text, _message.text, this);
    }

    public void DestroyMsg()
    {
        Destroy(gameObject);
    }

    public void StikeOutMsg()
    {
        _banned = true;
        _message.textStyle = TMP_Settings.defaultStyleSheet.GetStyle("Strike");
    }

    // ============== Helpers ==============
    public float GetTimeAlive()
    {
        return _aliveTime;
    }
}
