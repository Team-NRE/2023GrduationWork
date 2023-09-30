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


    //GameObject objectName;

    float defence = default;
    float invincibility_Time = default;
    float shield_Time = default;
    float Save_Health;

    int _userId;
    int _enemyLayer;

    float time = 0.01f;

    bool stop = false;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        _userId = userId;
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 1.12f, 0);
        defence = 9999;
        invincibility_Time = 1.5f;
        shield_Time = 3.0f;
    }

    public void Update()
    {
        if (stop == false)
        {
            //StartInvincibility();
            _pv.RPC("StartInvincibility", RpcTarget.All, _userId);
        }

        if (stop == true)
        {
            //Invoke("StartShield", 0.02f);
            DelayTimer(0.02f);
            _pv.RPC("StartShield", RpcTarget.All, _userId);
        }
    }


    [PunRPC]
    public void StartInvincibility(int playerId)
    {
        GameObject target = Managers.game.RemoteTargetFinder(playerId);
        if (target.tag == "PLAYER") { _pStats = target.GetComponent<PlayerStats>(); }
        if (target.tag != "PLAYER") { _oStats = target.GetComponent<ObjStats>(); }

        time += Time.deltaTime;

        if (time >= invincibility_Time)
        {
            if(target.tag == "PLAYER")
            { 
                //플레이어 방어력 빠짐
                _pStats.defensePower -= defence;

                Save_Health = _pStats.maxHealth / 100 * 25;
                _pStats.nowHealth += Save_Health;
            }

            if (target.tag != "PLAYER")
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

    [PunRPC]
    public void StartShield(int playerId)
    {
        GameObject target = Managers.game.RemoteTargetFinder(playerId);

        if (target.tag == "PLAYER") { _pStats = target.GetComponent<PlayerStats>(); }
        if (target.tag != "PLAYER") { _oStats = target.GetComponent<ObjStats>(); }
        time += Time.deltaTime;

        if (time >= shield_Time)
        {
            if(target.tag == "PLAYER") { _pStats.nowHealth -= Save_Health; }
            if(target.tag != "PLAYER") { _oStats.nowHealth -= Save_Health; }

            stop = false;
            time = 0;
            //PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
