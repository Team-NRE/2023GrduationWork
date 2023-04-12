using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyBar : MonoBehaviour
{
    Component stats;

    [SerializeField]
    [Range(0, 4)]
    private int maxEnegy;
    [SerializeField]
    [Range(0.0f, 4.0f)]
    private float enegy;

    [Space(20f)]
    [SerializeField]
    private Image[] enegyImages;

    private void Start() 
    {
        GetStatsScript();
    }

    private void Update() 
    {
        for (int i=0; i<enegyImages.Length; i++)
        {
            if (i >= maxEnegy)
                enegyImages[i].transform.parent.gameObject.SetActive(false);
            else
                enegyImages[i].transform.parent.gameObject.SetActive(true);

            if (enegy > maxEnegy)
                enegy = maxEnegy;

            enegyImages[i].fillAmount = enegy - i;
        }
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
