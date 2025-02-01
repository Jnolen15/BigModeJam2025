using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Start is called before the first frame update
    void Start()
    {
        SetFillPercent(33);
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


}
