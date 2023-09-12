using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;
using Photon.Pun;

public class LightsaberHpBar : MonoBehaviour
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

    private PlayerStats _pStats;
    private Transform cam;
    private float maxHealth;
    private float nowHealth;

    private static float DELAY_TIME = 0.5f;
    PhotonView _pv;

    public void Awake() 
    {
        cam = Camera.main.transform;
        _pStats = GetComponentInParent<PlayerStats>();

        maxHealth = _pStats.maxHealth;
        nowHealth = _pStats.nowHealth;

        healthBarHeal = transform.GetChild(1).gameObject.GetComponent<Image>();
        healthBarHit = transform.GetChild(2).gameObject.GetComponent<Image>();
        healthBarBasic = transform.GetChild(3).gameObject.GetComponent<Image>();
    }

    private void FixedUpdate() 
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

        if (nowHealth < _pStats.nowHealth)
        {
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

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

        if (nowHealth > _pStats.nowHealth)
        {
            nowHealth = _pStats.nowHealth;
            maxHealth = _pStats.maxHealth;

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
