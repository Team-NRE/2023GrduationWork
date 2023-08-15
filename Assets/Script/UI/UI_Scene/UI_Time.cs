using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Time : UI_Popup
{
    string sec;
    string min;
    double time;

    TextMeshProUGUI timeText;

    public override void Init()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        sec = "00";
        min = "00";
    }

    public override void UpdateInit()
    {
        time = Managers.game.playTime;

        min = ((int)time / 60).ToString();
        sec = ((int)time % 60).ToString();

        if (min.Length == 1) min = "0" + min;
        if (sec.Length == 1) sec = "0" + sec;

        timeText.text = $"{min}:{sec}";
    }
}
