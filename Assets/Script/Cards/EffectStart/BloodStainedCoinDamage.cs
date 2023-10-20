using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStainedCoinDamage : BaseEffect
{
    PhotonView _pv;

    float damage;
    int _playerId;
    int _targetId;
    ObjStats oStat;
    PlayerStats pStat;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);

        damage = 10.0f;
        _playerId = userId;
        _targetId = targetId;

        this.gameObject.transform.parent = target.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.8f, 0);

        if (target.gameObject.CompareTag("PLAYER"))
        {
            pStat = target.GetComponent<PlayerStats>();
            pStat.GetComponent<PlayerStats>().receviedDamage = (_playerId, damage);
        }
        else
        {
            oStat = target.GetComponent<ObjStats>();
            oStat.GetComponent<ObjStats>().nowHealth -= damage;
        }

        DelayTimer(2.0f);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
