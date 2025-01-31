using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextChatMsg : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private TextMeshProUGUI _timeStamp;
    [SerializeField] private TextMeshProUGUI _chatterName;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Image _buddyIcon;
    [SerializeField] private Sprite _moderatorIcon;
    
    private float _aliveTime;
    private ChatManager _chatMan;
    private bool _notClickable;

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

    public void SetupModMsg(string timeStamp, string msg, ChatManager chatMngr)
    {
        _notClickable = true;
        _timeStamp.text = timeStamp;
        _chatterName.text = "Moderator Action:";
        _chatterName.color = Color.red;
        _message.text = msg;
        _chatMan = chatMngr;

        _buddyIcon.sprite = _moderatorIcon;
        _buddyIcon.color = Color.red;
    }

    // ============== Function ==============
    private void Update()
    {
        _aliveTime += Time.deltaTime;
    }

    public void OnClick()
    {
        if (_notClickable)
            return;

        OnChatClicked?.Invoke(ChatTabsManager.BuddyStatus.Non_Buddy, _chatterName.text, _timeStamp.text, _message.text, this);
    }

    public void DestroyMsg()
    {
        Destroy(gameObject);
    }

    public void StikeOutMsg()
    {
        _notClickable = true;
        _message.textStyle = TMP_Settings.defaultStyleSheet.GetStyle("Strike");
    }

    // ============== Helpers ==============
    public float GetTimeAlive()
    {
        return _aliveTime;
    }
}
