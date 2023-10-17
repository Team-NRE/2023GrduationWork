using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeStart : BaseEffect
{
    PhotonView _pv;
    float _bulletSpeed;
    int _playerId;
    int _enemyLayer;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _bulletSpeed =10.0f;
        _damage = 100.0f;
        _playerId = userId;
        _enemyLayer = player.GetComponent<PlayerStats>().enemyArea;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);
        this.gameObject.transform.parent = null;
    }

    void OnTriggerEnter(Collider coll)
    {
        int targetId = Managers.game.RemoteColliderId(coll);
        if (targetId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, _playerId, targetId);
    }

    // Update is called once per frame
    void Update()
    {
        MoveParticle(_playerId);
    }

    [PunRPC]
    public void RpcTrigger(int playerId, int targetId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(targetId);
        PlayerStats pStats = Managers.game.RemoteTargetFinder(playerId).GetComponent<PlayerStats>();

        if (other.gameObject.layer == _enemyLayer)
        {
            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();

                oStats.nowHealth -= _damage + (pStats.basicAttackPower * 0.7f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (_playerId, _damage + (pStats.basicAttackPower * 0.7f));
            }
        }
    }

    [PunRPC]
    public void MoveParticle(int userId)
    {
        GameObject user = Managers.game.RemoteTargetFinder(userId);
        //Vector3 SpearDirection = playerTr.forward;
        Vector3 SpearDirection = user.transform.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * _bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }
}
