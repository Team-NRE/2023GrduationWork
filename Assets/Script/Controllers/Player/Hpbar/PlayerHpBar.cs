using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;
using Photon.Pun;

public class PlayerHpBar : MonoBehaviour
{
    [Header("- Health Bar Images")]
    [SerializeField]
    private Image healthBarOverShield;
    [SerializeField]
    private Image healthBarShield;
    [SerializeField]
    private Image healthBarHeal;
    [SerializeField]
    private Image healthBarHit;
    [SerializeField]
    private Image healthBarBasic;

    [Space(10f)]
    [Header("- Options")]
    [SerializeField]
    private bool isHealHitEffect = true;
    private bool isShieldHitEffect = true;

    private PlayerStats _pStats;
    private Transform cam;

    private float maxHealth;
    private float nowHealth;
    private float nowShield;

    PhotonView _pv;
    private static float DELAY_TIME = 0.5f;

    public void Awake()
    {
        cam = Camera.main.transform;
        _pStats = GetComponentInParent<PlayerStats>();

        maxHealth = _pStats.maxHealth;
        nowHealth = _pStats.nowHealth;
        nowShield = _pStats.shield;


        healthBarOverShield = transform.Find("HealthBarOverShieldEffect").gameObject.GetComponent<Image>();
        healthBarShield = transform.Find("HealthBarShieldEffect").gameObject.GetComponent<Image>();
        healthBarHeal = transform.Find("HealthBarHealEffect").gameObject.GetComponent<Image>();
        healthBarHit = transform.Find("HealthBarHitEffect").gameObject.GetComponent<Image>();
        healthBarBasic = transform.Find("HealthBar").gameObject.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (Managers.game.myCharacter != null && healthBarBasic.color == Color.white)
        {
            Color color;

            if (Managers.game.myCharacter.layer != transform.parent.gameObject.layer)
                ColorUtility.TryParseHtmlString("#FF5555", out color);
            else if (Managers.game.myCharacter != transform.parent.gameObject)
                ColorUtility.TryParseHtmlString("#5656FF", out color);
            else
                ColorUtility.TryParseHtmlString("#37FF37", out color);

            healthBarBasic.color = color;
        }

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

        if(nowShield < _pStats.shield)
        {
            nowShield = _pStats.shield;
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

            if (isShieldHitEffect)
            {
                UIChangeShield();
                Invoke("UIChangeHit", DELAY_TIME);
            }
            else
            {
                UIChangeShield();
                UIChangeHit();
            }
        }

        if(nowShield > _pStats.shield)
        {
            nowShield = _pStats.shield;
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

            if (isShieldHitEffect)
            {
                UIChangeHit();
                Invoke("UIChangeShield", DELAY_TIME);
            }
            else
            {
                UIChangeHit();
                UIChangeShield();
            }
        }


        if (nowHealth < _pStats.nowHealth)
        {
            nowShield = _pStats.shield;
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

            if (isHealHitEffect)
            {
                UIChangeHeal();
                Invoke("UIChangeBasic", DELAY_TIME);
                Invoke("UIChangeShield", DELAY_TIME);
                Invoke("UIChangeHit", DELAY_TIME);
            }
            else
            {
                UIChangeHeal();
                UIChangeBasic();
                UIChangeShield();
                UIChangeHit();
            }
        }

        if (nowHealth > _pStats.nowHealth)
        {
            nowShield = _pStats.shield;
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

            if (isHealHitEffect)
            {
                UIChangeBasic();
                UIChangeShield();
                UIChangeHeal();
                Invoke("UIChangeHit", DELAY_TIME);
            }
            else
            {
                UIChangeBasic();
                UIChangeShield();
                UIChangeHeal();
                UIChangeHit();
            }
        }
    }

    private void UIChangeBasic()
    {
        healthBarBasic.fillAmount = nowHealth / maxHealth;
    }

    private void UIChangeShield() 
    {
        if((nowHealth + nowShield) > maxHealth)
        {
            healthBarShield.fillAmount = (nowHealth + nowShield) / maxHealth;
            healthBarOverShield.fillAmount = (nowHealth + nowShield) / maxHealth - 1;
        }

        if((nowHealth + nowShield) <= maxHealth)
        {
            healthBarShield.fillAmount = (nowHealth + nowShield) / maxHealth;
            healthBarOverShield.fillAmount = (nowHealth + nowShield) / maxHealth - 1;
        }

    }

    private void UIChangeHeal()
    {
        healthBarHeal.fillAmount = nowHealth / maxHealth;
    }
    private void UIChangeHit()
    {
        if ((nowHealth + nowShield) > maxHealth)
        {
            healthBarOverShield.fillAmount = (nowHealth + nowShield) / maxHealth - 1;
            healthBarHit.fillAmount = (nowHealth + nowShield) / maxHealth;
        }

        if ((nowHealth + nowShield) <= maxHealth)
        {
            healthBarHit.fillAmount = (nowHealth + nowShield) / maxHealth;
            healthBarOverShield.fillAmount = 0;
        }
    }
}

