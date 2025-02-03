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
    [SerializeField] private TextMeshProUGUI _viewerCountTextBox;
    [SerializeField] private Image _buddyStatusImage;
    [SerializeField] private GameObject _modToolsTab;
    [SerializeField] private GameObject _rulesTab;
    [SerializeField] private GameObject _reportsTab;
    [SerializeField] private GameObject _rulesNotification;
    [SerializeField] private GameObject _reportsNotification;
    [SerializeField] private GameObject _modNotification;
    [SerializeField] private GameObject _messageNotFound;
    [SerializeField] private GameObject _modToolsIndicator;
    [SerializeField] private GameObject _rulesIndicator;
    [SerializeField] private GameObject _reportsIndicator;


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
    // ===================== Setup =====================
    void Start()
    {
        TextChatMsg.OnChatClicked += SetTargetAccount;

        // starting on rules tab
        _modToolsTab.SetActive(false);
        _rulesTab.SetActive(true);

        // disabling notification bulles
        _modNotification.SetActive(false);
        _rulesNotification.SetActive(false);
        _reportsNotification.SetActive(false);

        // resetting mod tab to neutral
        _selectedMsg = null;
        _accountNameTextBox.text = "---";
        _messageTextBox.text = "---";
        _TimeStampTextBox.text = "-:--";
        _buddyStatusImage.gameObject.SetActive(false);

        // initializing rules list
        GenerateRuleList();

        // initializing viewer count coroutine
        StartCoroutine(ManageViewerCount());
    }

    public void OnDestroy()
    {
        TextChatMsg.OnChatClicked -= SetTargetAccount;
    }

    // ===================== Mod Action =====================
    public void SetTargetAccount(TextChatMsg chatMessage) // passing through and storing text data structure
    {
        // setting target message
        _selectedMsg = chatMessage;
        // ascribing data to text boxes
        _accountNameTextBox.text = chatMessage.GetAccountName();
        _accountNameTextBox.color = chatMessage.GetAccountNameColor();
        _messageTextBox.text = chatMessage.GetMessageText();
        _TimeStampTextBox.text = chatMessage.GetTimeStamp();

        // setting icon
        _buddyStatusImage.gameObject.SetActive(true);
        switch (chatMessage.GetBuddyStatus()) // setting buddy image based on enum reveived
        {
            case BuddyStatus.Non_Buddy:
                _buddyStatusImage.sprite = _nonBuddyIcon;
                break;
            case BuddyStatus.Buddy:
                _buddyStatusImage.sprite = _buddyIcon;
                break;
            case BuddyStatus.VIB:
                _buddyStatusImage.sprite = _veryImportantBuddyIcon;
                break;

            default:
                Debug.Log("Invalid buddy status");
                break;
        }

        OpenModTab();
        //ToggleTab(_modToolsTab);
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

        CloseModTab();
        // resetting mod tab to neutral
        _selectedMsg = null;
        _accountNameTextBox.text = "---";
        _messageTextBox.text = "---";
        _TimeStampTextBox.text = "-:--";
        _buddyStatusImage.gameObject.SetActive(false);
    }

    // ===================== Tab Stuff =====================
    public void Update()
    {
        if (!_modToolsTab.activeSelf)
            return;

        if (_selectedMsg == null)
        {
            _messageNotFound.SetActive(true);
        }
    }

    public void ToggleRulesTab()
    {
        // Clicking the rules tab should toggle it and close the mod tab
        _rulesTab.SetActive(!_rulesTab.activeSelf);
        _modToolsTab.SetActive(false);
        ToggleTabIndicators(_rulesIndicator);
    }

    public void OpenModTab()
    {
        // Mod tab cant be clicked in scene, but when called hide rules (for RN)
        _modToolsTab.SetActive(true);
        _rulesTab.SetActive(false);
        _messageNotFound.SetActive(false);
        ToggleTabIndicators(_modToolsIndicator);
    }

    public void CloseModTab()
    {
        _modToolsTab.SetActive(false);
    }

    public void ToggleReportsTab()
    {
        // Reports tab should never close. Clicking the button should close the other tabs
        _modToolsTab.SetActive(false);
        _rulesTab.SetActive(false);
        ToggleTabIndicators(_reportsIndicator);
    }

    private void ToggleTabIndicators(GameObject indicator)
    {
        _modToolsIndicator.SetActive(false);
        _reportsIndicator.SetActive(false);
        _rulesIndicator.SetActive(false);

        indicator.SetActive(true);
    }

    // OBSELETE
    public void ToggleTab(GameObject tab) // toggles selected tab and closes all other ones that are open
    {
        tab.SetActive(!tab.activeSelf);

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

    public void OpenTab(GameObject tab) // opens selected tab and closes all others
    {
        if (tab == _rulesTab)
            ToggleTabIndicators(_rulesIndicator);
        else if (tab == _modToolsTab)
            ToggleTabIndicators(_modToolsIndicator);
        else if (tab == _reportsTab)
            ToggleTabIndicators(_reportsIndicator);

        tab.SetActive(true);
        // toggling other tabs closed
        var tempArray = new GameObject[] {_rulesTab, _modToolsTab};
        foreach (GameObject temp in tempArray)
        {
            if (temp.activeSelf && tab != temp) temp.SetActive(false);
        }
    }

    // ===================== Other UI Function =====================
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
        if (!_rulesTab.activeSelf) ActivateNotification(_rulesNotification);
    }

    public void AddRule(string s, PunishementType p) // adds rule to end of the list
    {
        RulesList.Add(s + " - " + PunishmentTypeToString(p));
        GenerateRuleList();
    }

    public void RemoveRule(string s, PunishementType p) // removes a specific rule from the list
    {
        RulesList.Remove(s + " - " + PunishmentTypeToString(p));
        GenerateRuleList();
    }

    public void ShowNotificationOnReportTab()
    {
        // reports is always open so you have to check if others aren't open
        if (!_rulesTab.activeSelf && !_modToolsTab.activeSelf) ActivateNotification(_reportsNotification);
    }

    public void ActivateNotification(GameObject g) // activates notification icon (gameobject)
    {
        g.SetActive(true);
    }

    public void HideNotification(GameObject g) // hides notification icon
    {
         if(g.activeSelf) g.SetActive(false);
    }

    private IEnumerator ManageViewerCount()
    {

        float startingViewerCount = 50;
        float viewerCount = startingViewerCount;
        float viewerIncreaseRate = 1.05f; // rate at which the # of viewers increases
        float viewerCountIncreaseRate = 0.99f; // rate at which the time between viewer count increases
        float maxViewerCountIncrease = 3;
        float minViewerCountIncrease = 1;
        float maxViewerIncreaseTime = 10;
        float minViewerIncreaseTime = 1;
        _viewerCountTextBox.text = viewerCount.ToString();
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minViewerIncreaseTime, maxViewerIncreaseTime));
            //increasing viewers
            viewerCount += UnityEngine.Random.Range(minViewerCountIncrease, maxViewerCountIncrease);
            _viewerCountTextBox.text = MathF.Floor(viewerCount).ToString();
            
            // ramping up # of viewers that join and the space in between them joining
            maxViewerIncreaseTime *= viewerCountIncreaseRate;
            minViewerIncreaseTime *= viewerCountIncreaseRate;
            minViewerCountIncrease *= viewerIncreaseRate;
            maxViewerCountIncrease *= viewerCountIncreaseRate;
        }
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

    private string PunishmentTypeToString(PunishementType p)
    {
        switch (p)
        {
            case PunishementType.Timeout:
                return "Timeout";
            case PunishementType.Temp_Ban:
                return "Temp Ban";
            case PunishementType.Perma_Ban:
                return "Perma Ban";
            case PunishementType.Make_NonBuddy:
                return "Make Non-Buddy";
            case PunishementType.Make_Buddy:
                return "Make Buddy";
            case PunishementType.Make_VIB:
                return "Make VIB";

            default:
                Debug.LogWarning("Invalid int for enum conversion");
                return "invalid punishment type";
        }
    }
}
