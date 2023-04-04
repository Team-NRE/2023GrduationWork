using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
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
    private Stats stats;
    private float maxHealth;
    private float nowHealth;

    private static float DELAY_TIME = 0.5f;
    
    private void Awake() 
    {
        cam = Camera.main.transform;
        
        GetStatsScript();

        maxHealth = stats.MaxHealth;
        nowHealth = stats.NowHealth;
    }

    private void FixedUpdate() 
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

        if (nowHealth < stats.NowHealth)
        {
            nowHealth = stats.NowHealth;
            maxHealth = stats.MaxHealth;

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

        if (nowHealth > stats.NowHealth)
        {
            nowHealth = stats.NowHealth;
            maxHealth = stats.MaxHealth;

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

    private void GetStatsScript()
    {
        GameObject obj = gameObject;

        while (stats == null) 
        {
            obj = obj.transform.parent.gameObject;
            stats = obj?.GetComponent<Stats>();
        }
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
