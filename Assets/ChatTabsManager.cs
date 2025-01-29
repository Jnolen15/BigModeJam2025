using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChatTabsManager : MonoBehaviour
{
    [SerializeField] const string heh = "";
    [SerializeField] private TextMeshProUGUI _accountNameTextBox;
    [SerializeField] private GameObject _modToolsTab;
    [SerializeField] private GameObject _rulesTab;
    [SerializeField] private String _targetAccountName;

    enum PunishementType
    {
        Timeout,
        Temp_Ban,
        Perma_Ban,
        Make_NonBuddy,
        Make_Buddy,
        Make_VIB
    }

    public delegate void ChatTabManagerEvent(int i);
        private static event ChatTabManagerEvent OnModEvent;


    void Start()
    {
        _modToolsTab.SetActive(false);
        _rulesTab.SetActive(true);
    }

    public void SetTargetAccount(string s)
    {
        _targetAccountName = s;
        _accountNameTextBox.text = s;
    }

    public void ModAction(int i)
    {
        Debug.Log("Invoking mod action " + i + " on account: " + _targetAccountName);
        OnModEvent?.Invoke(i);
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
}
