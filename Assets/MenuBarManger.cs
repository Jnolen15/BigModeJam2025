using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarManger : MonoBehaviour
{
    // Fill goes from -max to max

    [SerializeField] float StartingFillPercent = 50;
    [SerializeField] TextMeshProUGUI _numberText;
    [SerializeField] Image _fillImage;
    [SerializeField] float _maxValue = 100;
    [SerializeField] float _minValue = -100;
    [SerializeField] BarType _barType;

    public enum BarType
    {
        ScoreBar,
        Chatbar
    }

    // Start is called before the first frame update
    public void Start()
    {
        ChatManager.OnScoreUpdated += SetScore;

        _fillImage.fillAmount = 0.5f;
    }

    public void OnDestroy()
    {
        ChatManager.OnScoreUpdated -= SetScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigureBar(float f) //set max bar value
    {
        _maxValue = f;
    }

    public void SetFillPercent(float percent) // sets the fill and bar based on 
    {
        _fillImage.fillAmount = percent / 100;
        float sum = _maxValue * 2 * percent/100 - _maxValue;
        string s = "";
        if (sum >= 0) s += "+"; // adding plus sign if positive
        _numberText.text = s += sum.ToString();
    }

    public void SetScore(int currentScore, int maxScore)
    {
        // setting score text
        string s = "";
        if (currentScore >= 0) s += "+"; // adding plus sign if positive
        _numberText.text = s + currentScore.ToString();

        // setting fill amount
        _fillImage.fillAmount = (float)(maxScore + currentScore) / (float)(maxScore * 2);
    }


}
