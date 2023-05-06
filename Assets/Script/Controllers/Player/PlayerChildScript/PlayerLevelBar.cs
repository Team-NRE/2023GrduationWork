using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
public class PlayerLevelBar : MonoBehaviour
{
    Component stats;

    [SerializeField]
    private TextMeshProUGUI text;

    [Range (0, 99)]
    [SerializeField]
    private int level;

    private void Start() 
    {
        GetStatsScript();
    }

    private void Update()
    {
        text.text = level.ToString();
    }

    private void GetStatsScript()
    {
        GameObject obj = gameObject;

        while (stats == null) 
        {
            obj = obj.transform.parent.gameObject;
            if(obj.tag == "PLAYER") { stats = obj?.GetComponent<PlayerStats>(); }
        }
    }
}
*/