using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class ChatTabsManager : MonoBehaviour
{
    [SerializeField] const string heh = "";
    [SerializeField] private TextMeshProUGUI _accountNameTextBox;
    [SerializeField] private TextMeshProUGUI _messageTextBox;
    [SerializeField] private TextMeshProUGUI _TimeStampTextBox;
    [SerializeField] private TextMeshProUGUI _rulesTextBox;
    [SerializeField] private GameObject _modToolsTab;
    [SerializeField] private GameObject _rulesTab;
    [SerializeField] private String _targetAccountName;
    public List<string> RulesList = new List<string>();

    public enum PunishementType
    {
        Timeout,
        Temp_Ban,
        Perma_Ban,
        Make_NonBuddy,
        Make_Buddy,
        Make_VIB
    }

    public delegate void ChatTabManagerEvent(PunishementType p);
    private static event ChatTabManagerEvent OnModEvent;


    // TODO add punishments to rules list

    void Start()
    {
        // starting on rules tab
        _modToolsTab.SetActive(false);
        _rulesTab.SetActive(true);

        // initializing rules list
        GenerateRuleList();

        // rules list testing
        AddRule("Don't be Cringe");
        AddRule("No spamming");
        AddRule("Coping is mandatory");

        RemoveRule("No spamming");
    }

    public void SetTargetAccount(string name, string timeStamp, string message)
    {
        _targetAccountName = name;
        _accountNameTextBox.text = name;
        _messageTextBox.text = "\"" + message + "\"";
        _TimeStampTextBox.text = timeStamp;
    }

    public void ModAction(int i)
    {
        Debug.Log("Invoking mod action " + IntToPunishmentType(i) + " on account: " + _targetAccountName);
        OnModEvent?.Invoke(IntToPunishmentType(i));
    }

    public void ToggleRulesTab() // toggles tab
    {
        _rulesTab.SetActive(!_rulesTab.activeSelf);
        // close other tabs if they're open
        if (_modToolsTab.activeSelf) _modToolsTab.SetActive(false); 
    }

    public void ToggleModTab() // toggles tab
    {
        _modToolsTab.SetActive(!_modToolsTab.activeSelf);
        // close other tabs if they're open
        if (_rulesTab.activeSelf) _rulesTab.SetActive(false);
    }

    private void GenerateRuleList()
    {
        string rulesList = "";

        int iterator = 0;
        foreach(string s in RulesList)
        {
            iterator += 1;
            rulesList += iterator + ") " + s + "\n";
        }

        _rulesTextBox.text = rulesList;
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
                Debug.Log("Invalid int for enum conversion");
                return PunishementType.Timeout;
        }
    }
}
