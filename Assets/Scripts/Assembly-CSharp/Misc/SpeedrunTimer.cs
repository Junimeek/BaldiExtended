using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    private void Start()
    {
        totalTime = 0.02f;
        curMiliseconds = 0.02f;
        curMinutes = 0;
        curSeconds = 0;
        timeText.text = "0:00.00";
    }

    private void Update()
    {
        if (allowTime)
        {
            totalTime += Time.unscaledDeltaTime;
            curMiliseconds += Time.unscaledDeltaTime;
            
            if (curMiliseconds >= 1)
            {
                curMiliseconds = 1f-curMiliseconds;
                curSeconds++;
            }

            if (curSeconds >= 60)
            {
                curSeconds = 0;
                curMinutes++;
            }

            if (curSeconds < 10) timeText.text = curMinutes +":0" + curSeconds + "." + (curMiliseconds*100f).ToString("0");
            else if (curSeconds >= 10) timeText.text = curMinutes +":" + curSeconds + "." + (curMiliseconds*100f).ToString("0");
        }
    }
    
    public float totalTime;
    public bool allowTime;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int curMinutes;
    [SerializeField] private int curSeconds;
    [SerializeField] private float curMiliseconds;
}
