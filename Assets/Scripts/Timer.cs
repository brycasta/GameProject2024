using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Lee - Whole Script
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limits")]
    public bool hasLimit;
    public float timerLimit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This makes the time go up or down depending on the countDown bool
        if (countDown == true)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
        //This sets the Limit
        if (hasLimit == true)
        {
            if((countDown == true && currentTime <= timerLimit) || (countDown == false && currentTime >= timerLimit))
            {
                currentTime = timerLimit;
                SetTimerText();
                timerText.color = Color.red;
                enabled = false;
            }
        }
        SetTimerText();
    }

    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("0.00");
    }
}
