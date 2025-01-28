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
    

    // ============== Setup ==============	
    public void Setup(string timeStamp, string chatterName, string msg)
    {
        _timeStamp.text = timeStamp;
        _chatterName.text = chatterName;
        _message.text = msg;

        // Something here for buddy badge
    }

    // ============== Function ==============
    public void OnClick()
    {
        Debug.Log("Clicked!!!");
    }
}
