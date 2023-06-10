using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;

public class PoliceHealthBar : MonoBehaviour
{
    [Header ("- Health Bar Images")]
    [SerializeField]
    private Image healthBarHeal;
    [SerializeField]
    private Image healthBarHit;
    [SerializeField]
    private Image healthBarBasic;

    [Space (10f)]
    [Header ("- Options")]
    [SerializeField]
    private bool isHealHitEffect = true;

    private Transform cam;
    public PlayerStats stats;
    private float maxHealth;
    private float nowHealth;

    private static float DELAY_TIME = 0.5f;
    
    private void Awake() 
    {
        cam = Camera.main.transform;
        
        GetStatsScript();

        maxHealth = stats.maxHealth;
        nowHealth = stats.nowHealth;

        healthBarHeal = transform.GetChild(1).gameObject.GetComponent<Image>();
        healthBarHit = transform.GetChild(2).gameObject.GetComponent<Image>();
        healthBarBasic = transform.GetChild(3).gameObject.GetComponent<Image>();
    }

    private void FixedUpdate() 
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

        if (nowHealth < stats.nowHealth)
        {
            nowHealth = stats.nowHealth;
            maxHealth = stats.maxHealth;

            if (isHealHitEffect)
            {
                UIChangeHeal();
                Invoke("UIChangeBasic", DELAY_TIME);
                Invoke("UIChangeHit", DELAY_TIME);
            }
            else
            {
                UIChangeHeal();
                UIChangeBasic();
                UIChangeHit();
            }
        }

        if (nowHealth > stats.nowHealth)
        {
            nowHealth = stats.nowHealth;
            maxHealth = stats.maxHealth;

            if (isHealHitEffect)
            {
                UIChangeBasic();
                UIChangeHeal();
                Invoke("UIChangeHit", DELAY_TIME);
            }
            else
            {
                UIChangeBasic();
                UIChangeHeal();
                UIChangeHit();
            }
        }
    }

    private Component GetStatsScript()
    {
        GameObject police = gameObject;
        

        while (stats == null) 
        {
            police = police.transform.parent.gameObject;
            if(police.tag == "PLAYER") { stats = police?.GetComponent<PlayerStats>(); }
        }

        return stats;
    }

    private void UIChangeBasic()
    {
        healthBarBasic.fillAmount = nowHealth / maxHealth;
    }
    private void UIChangeHeal()
    {
        healthBarHeal.fillAmount = nowHealth / maxHealth;
    }
    private void UIChangeHit()
    {
        healthBarHit.fillAmount = nowHealth / maxHealth;
    }
}
