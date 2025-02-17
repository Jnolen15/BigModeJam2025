using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class TextChatMsg : MonoBehaviour
{
    // ============== Refrences / Variables ==============
    [SerializeField] private TextMeshProUGUI _timeStamp;
    [SerializeField] private TextMeshProUGUI _chatterName;
    [SerializeField] private ChatTabsManager.BuddyStatus _buddyStatus = ChatTabsManager.BuddyStatus.Non_Buddy; // default to non-buddy
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Image _buddyImage;
    [SerializeField] private Sprite _moderatorIcon;
    [SerializeField] private Sprite _nonBuddyIcon;
    [SerializeField] private Sprite _buddyIcon;
    [SerializeField] private Sprite _vibIcon;
    [SerializeField] private Image _baseimg;
    [SerializeField] private Color _colorBase;
    [SerializeField] private Color _colorCotClickable;
    [SerializeField] private Color _colorhighlighted;

    private CommentsSO _commentSO;
    private UsernamesSO _usernameSO;

    private float _aliveTime;
    private ChatManager _chatMan;
    private bool _notClickable;

    public delegate void ChatEvent(TextChatMsg msg);
    public static event ChatEvent OnChatClicked;

    // ============== Setup ==============	
    public void Setup(UsernamesSO username, CommentsSO comment, string timeStamp, ChatManager chatMngr)
    {
        _commentSO = comment;
        _usernameSO = username;

        _timeStamp.text = timeStamp;
        _chatterName.text = username.GetUsername() + ":";
        _chatterName.color = username.ChatterColor;
        _message.text = comment.Message;
        _chatMan = chatMngr;

        // Something here for buddy badge
        // TODO make scriptable objects have buddy statuses instead of random
        switch (username.BuddyLevel)
        {
            case 1:
                _buddyStatus = ChatTabsManager.BuddyStatus.Non_Buddy;
                _buddyImage.sprite = _nonBuddyIcon;
                break;
            case 2:
                _buddyStatus = ChatTabsManager.BuddyStatus.Buddy;
                _buddyImage.sprite = _buddyIcon;
                break;
            case 3:
                _buddyStatus = ChatTabsManager.BuddyStatus.VIB;
                _buddyImage.sprite = _vibIcon;
                break;
            default:
                Debug.LogWarning("Invalid int for Buddy status conversion");
                _buddyStatus = ChatTabsManager.BuddyStatus.Non_Buddy;
                break;
        }
    }

    public void SetupModMsg(string timeStamp, string msg, ChatManager chatMngr)
    {
        _notClickable = true;
        _timeStamp.text = timeStamp;
        _chatterName.text = "Moderator Action:";
        _chatterName.color = Color.red;
        _message.text = msg;
        _chatMan = chatMngr;

        _buddyImage.sprite = _moderatorIcon;
        _buddyImage.color = Color.red;
        _baseimg.color = _colorCotClickable;
    }

    public void SetupReportMsg(UsernamesSO username, string comment, ChatTabsManager.BuddyStatus buddyStat, string timeStamp)
    {
        _notClickable = true;
        _timeStamp.text = timeStamp;
        _chatterName.text = username.GetUsername() + ":";
        _chatterName.color = username.ChatterColor;
        _message.text = comment;

        switch (buddyStat)
        {
            case ChatTabsManager.BuddyStatus.Non_Buddy:
                _buddyImage.sprite = _nonBuddyIcon;
                break;
            case ChatTabsManager.BuddyStatus.Buddy:
                _buddyImage.sprite = _buddyIcon;
                break;
            case ChatTabsManager.BuddyStatus.VIB:
                _buddyImage.sprite = _vibIcon;
                break;
            default:
                Debug.LogWarning("Invalid int for Buddy status conversion");
                _buddyStatus = ChatTabsManager.BuddyStatus.Non_Buddy;
                break;
        }
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

        // OnChatClicked?.Invoke(ChatTabsManager.BuddyStatus.Non_Buddy, _chatterName.text, _timeStamp.text, _message.text, this);
        OnChatClicked?.Invoke(this);

        _baseimg.color = _colorhighlighted;
    }

    public void ResetColor()
    {
        _baseimg.color = _colorBase;
    }

    public void DestroyMsg()
    {
        Destroy(gameObject);
    }

    public void StikeOutMsg()
    {
        _notClickable = true;
        _message.textStyle = TMP_Settings.defaultStyleSheet.GetStyle("Strike");
        _baseimg.color = _colorCotClickable;
    }

    public void VIBMsg()
    {
        _notClickable = true;
        _buddyImage.sprite = _vibIcon;
        _baseimg.color = _colorCotClickable;
    }

    // ============== Helpers ==============
    public CommentsSO GetComment()
    {
        return _commentSO;
    }

    public UsernamesSO GetUsername()
    {
        return _usernameSO;
    }

    public float GetTimeAlive()
    {
        return _aliveTime;
    }

    public string GetMessageText()
    {
        return _commentSO.Message;
    }

    public string GetTimeStamp()
    {
        return _timeStamp.text;
    }

    public string GetAccountName()
    {
        return _usernameSO.GetUsername();
    }

    public Color GetAccountNameColor()
    {
        return _usernameSO.ChatterColor;
    }

    public ChatTabsManager.BuddyStatus GetBuddyStatus()
    {
        return _buddyStatus;
    }

    public bool GetNotClickable()
    {
        return _notClickable;
    }
}
