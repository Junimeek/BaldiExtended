using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    private void Start()
    {
        curTime = 0f;
        timeText.text = "0:00.00";
    }

    private void Update()
    {
        if (allowTime)
        {
            curTime += Time.unscaledDeltaTime;
            timeText.text = curTime.ToString("0.00");
        }
    }

    
    public void SaveFinalTime(float time)
    {
        allowTime = false;

        // making my hot catgirl do this for me, thanks nat :3
        /*
        PlayerPrefs.SetFloat("LastTime", time);

        if (PlayerPrefs.GetFloat("LastTime") < PlayerPrefs.GetFloat("BestTime"))
        {
            PlayerPrefs.SetFloat("BestTime", time);
        }
        */
    }
    

    public float curTime;
    public bool allowTime;
    [SerializeField] private TMP_Text timeText;
}
