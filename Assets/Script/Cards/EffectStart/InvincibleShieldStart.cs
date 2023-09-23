using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using UnityEngine.UIElements;
using Photon.Pun;

public class InvincibleShieldStart : BaseEffect
{
    PlayerStats _pStats;
    ObjStats _oStats; 
    protected PhotonView _pv;


    GameObject objectName;

    float defence = default;
    float invincibility_Time = default;
    float shield_Time = default;
    float Save_Health;

    float time = 0.01f;

    bool stop = false;

    public override void CardEffectInit(int userId, int targetId)
    {
        base.CardEffectInit(userId);
        objectName = GetRemotePlayer(targetId);

        if (objectName.tag == "PLAYER") { _pStats = objectName.GetComponent<PlayerStats>(); }
        if (objectName.tag != "PLAYER") { _oStats = objectName.GetComponent<ObjStats>(); }

        defence = 10000;
        invincibility_Time = 1.5f;
        shield_Time = 3.0f;
    }

    public void Update()
    {
        if (stop == false)
        {
            StartInvincibility();
        }

        if (stop == true)
        {
            Invoke("StartShield", 0.02f);
        }
    }


    public void StartInvincibility()
    {
        time += Time.deltaTime;

        if (time >= invincibility_Time)
        {
            if(objectName.tag == "PLAYER")
            { 
                //플레이어 방어력 빠짐
                _pStats.defensePower -= defence;

                Save_Health = _pStats.maxHealth / 100 * 25;
                _pStats.nowHealth += Save_Health;
            }

            if (objectName.tag != "PLAYER")
            {
                //플레이어 방어력 빠짐
                _oStats.defensePower -= defence;

                Save_Health = _oStats.maxHealth / 100 * 25;
                _oStats.nowHealth += Save_Health;
            }

            stop = true;
            time = 0;
        }
    }

    public void StartShield()
    {
        time += Time.deltaTime;

        if (time >= shield_Time)
        {
            if(objectName.tag == "PLAYER") { _pStats.nowHealth -= Save_Health; }
            if(objectName.tag != "PLAYER") { _oStats.nowHealth -= Save_Health; }

            stop = false;
            time = 0;
            Destroy(this.gameObject);
        }
    }
}
