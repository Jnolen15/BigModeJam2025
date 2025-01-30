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
    

    // ============== Setup ==============	
    public void Setup(string timeStamp, string chatterName, string msg, ChatManager chatMngr)
    {
        _timeStamp.text = timeStamp;
        _chatterName.text = chatterName;
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
        Debug.Log("Clicked!!!");
        _chatMan.BanMe(this);
    }

    public void DestroyMsg()
    {
        Destroy(gameObject);
    }

    public float GetTimeAlive()
    {
        return _aliveTime;
    }
}
