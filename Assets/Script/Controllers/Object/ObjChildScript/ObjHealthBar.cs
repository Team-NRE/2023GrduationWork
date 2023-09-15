using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;
using Define;

public class ObjHealthBar : MonoBehaviour
{
    [Header ("- parent")]
    GameObject parent;

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
    public ObjStats stats;
    private float maxHealth;
    private float nowHealth;

    private static float DELAY_TIME = 0.5f;
    
    private void Awake() 
    {
        GetParent();
        cam = Camera.main.transform;
        
        GetStatsScript();

        maxHealth = stats.maxHealth;
        nowHealth = stats.nowHealth;
    }

    private void FixedUpdate() 
    {
        if (Managers.game.myCharacter != null && healthBarBasic.color == Color.white)
        {
            Color color;
            ColorUtility.TryParseHtmlString(
                (parent.layer == Managers.game.myCharacter.layer ? "#5656FF" : "#FF5555")
                , out color
            );
            
            healthBarBasic.color = color;
        }

        transform.LookAt(transform.position + cam.rotation * Vector3.back, cam.rotation * Vector3.up);

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

    private void GetParent()
    {
        parent = gameObject;

        while (parent.layer == (int)Layer.UI && parent.layer == (int)Layer.Default)
            parent = parent.transform.parent.gameObject;
    }

    private Component GetStatsScript()
    {
        GameObject obj = gameObject;
        

        while (stats == null) 
        {
            obj = obj.transform.parent.gameObject;
            if(obj.tag != "PLAYER") { stats = obj?.GetComponent<ObjStats>(); }
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
