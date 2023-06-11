using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Time : UI_Popup
{
    int sec;
    int min;
    float time;

    TextMeshProUGUI timeText;

    public override void Init()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        sec = 0;
        min = 0;
    }

    public override void UpdateInit()
    {
        time += Time.deltaTime;

        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (sec >= 60)
        {
            min += 1;
            sec -= 60;
        }
        else
        {
            timeText.text = $"{min.ToString()}:{sec.ToString()}";
        }
    }
}
