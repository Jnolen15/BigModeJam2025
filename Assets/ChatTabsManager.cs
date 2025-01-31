using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Playables;

public class ChatTabsManager : MonoBehaviour
{
    [SerializeField] const string heh = ""; // lmao
    // Object/Component References
    [SerializeField] private TextMeshProUGUI _accountNameTextBox;
    [SerializeField] private TextMeshProUGUI _messageTextBox;
    [SerializeField] private TextMeshProUGUI _TimeStampTextBox;
    [SerializeField] private TextMeshProUGUI _rulesTextBox;
    [SerializeField] private Image _buddyStatusImage;
    [SerializeField] private GameObject _modToolsTab;
    [SerializeField] private GameObject _rulesTab;
    [SerializeField] private GameObject _reportsTab;
    [SerializeField] private GameObject _rulesNotification;
    [SerializeField] private GameObject _reportsNotification;
    [SerializeField] private GameObject _modNotification;


    // Asset references
    [SerializeField] private Sprite _nonBuddyIcon;
    [SerializeField] private Sprite _buddyIcon;
    [SerializeField] private Sprite _veryImportantBuddyIcon;
    public List<string> RulesList = new List<string>();

    private TextChatMsg _selectedMsg;

    public enum PunishementType
    {
        Timeout,
        Temp_Ban,
        Perma_Ban,
        Make_NonBuddy,
        Make_Buddy,
        Make_VIB
    }

    public enum BuddyStatus
    {
        Non_Buddy,
        Buddy,
        VIB
    }

    public delegate void ChatTabManagerEvent(PunishementType p, TextChatMsg chatMsg);
    public static event ChatTabManagerEvent OnModEvent;


    // TODO add punishments to rules list

    void Start()
    {
        TextChatMsg.OnChatClicked += SetTargetAccount;

        // starting on rules tab
        _modToolsTab.SetActive(false);
        _reportsTab.SetActive(false);
        _rulesTab.SetActive(true);

        // disabling notification bulles
        _modNotification.SetActive(false);
        _rulesNotification.SetActive(false);
        _reportsNotification.SetActive(false);

        // initializing rules list
        GenerateRuleList();

        // rules list testing
        AddRule("Don't be Cringe");
        AddRule("No spamming");
        AddRule("Coping is mandatory");

        RemoveRule("No spamming");
    }

    public void OnDestroy()
    {
        TextChatMsg.OnChatClicked -= SetTargetAccount;
    }

    public void SetTargetAccount(TextChatMsg chatMessage) // passing through and storing text data structure
    {

        _selectedMsg = chatMessage;
        _accountNameTextBox.text = chatMessage.GetAccountName();
        _messageTextBox.text = "\"" + chatMessage.GetMessageText() + "\"";
        _TimeStampTextBox.text = chatMessage.GetTimeStamp();

        switch (chatMessage.GetBuddyStatus()) // setting buddy image based on enum reveived
        {
            case BuddyStatus.Buddy:
                _buddyStatusImage.sprite = _nonBuddyIcon;
                break;
            case BuddyStatus.Non_Buddy:
                _buddyStatusImage.sprite = _buddyIcon;
                break;
            case BuddyStatus.VIB:
                _buddyStatusImage.sprite = _veryImportantBuddyIcon;
                break;

            default:
                Debug.Log("Invalid buddy status");
                break;
        }

        ToggleTab(_modToolsTab);
    }

    public void ModAction(int i)
    {
        if (_selectedMsg == null)
        {
            Debug.LogWarning("Targeted message no longer exists");
        } else
        {
            Debug.Log("Invoking mod action " + IntToPunishmentType(i) + " on account: " + _selectedMsg.GetAccountName());
        }
        OnModEvent?.Invoke(IntToPunishmentType(i), _selectedMsg);
        ToggleTab(_modToolsTab);
    }

    public void ToggleTab(GameObject tab) // toggles selected tab and closes all other ones that are open
    {
        tab.SetActive(!tab.activeSelf);

        // closing notifications
        // TODO: close notifications on CLOSE, not open

        // toggling other tabs closed
        var tempArray = new GameObject[] { _rulesTab, _modToolsTab, _reportsTab };
        foreach (GameObject temp in tempArray)
        {
            if (temp.activeSelf && tab != temp) temp.SetActive(false);
        }

        // this might be more efficient
        //GameObject temp = _rulesTab;
        //if (temp.activeSelf && tab != temp) temp.SetActive(false);
        //temp = _modToolsTab;
        //if (temp.activeSelf && tab != temp) temp.SetActive(false);
        //temp = _reportsTab;
        //if (temp.activeSelf && tab != temp) temp.SetActive(false);
    }

    private void GenerateRuleList()
    {
        // generating string
        string rulesList = "";
        int iterator = 0;
        foreach(string s in RulesList)
        {
            iterator += 1;
            rulesList += iterator + ") " + s + "\n";
        }

        _rulesTextBox.text = rulesList;

        // Activating tab notification
        ActivateNotification(_rulesNotification);
    }

    public void AddRule(string s) // adds rule to end of the list
    {
        RulesList.Add(s);
        GenerateRuleList();
    }

    public void RemoveRule(string s) // removes a specific rule from the list
    {
        RulesList.Remove(s);
        GenerateRuleList();
    }

    public void ActivateNotification(GameObject g) // activates notification icon (gameobject)
    {
        g.SetActive(true);
    }

    public void HideNotification(GameObject g) // hides notification icon
    {
         if(g.activeSelf) g.SetActive(false);
    }




    private PunishementType IntToPunishmentType(int i)
    {
        switch(i)
        {
            case 0:
                return PunishementType.Timeout;
            case 1:
                return PunishementType.Temp_Ban;
            case 2:
                return PunishementType.Perma_Ban;
            case 3:
                return PunishementType.Make_NonBuddy;
            case 4:
                return PunishementType.Make_Buddy;
            case 5:
                return PunishementType.Make_VIB;

            default:
                Debug.LogWarning("Invalid int for enum conversion");
                return PunishementType.Timeout;
        }
    }
}
