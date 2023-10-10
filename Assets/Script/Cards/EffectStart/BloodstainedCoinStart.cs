using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;

public class BloodstainedCoinStart : BaseEffect
{
    GameObject _effectObject;
    protected PhotonView _pv;
    protected int _playerId;
    protected int _targetId;

    //Transform target = null;

    float damage = default;

    PlayerStats enemyStats;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);

        _playerId = userId;
        _targetId = targetId;
        
        damage = 10.0f;

        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId, _targetId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId, int targetId)
	{
        if (target == null)
        {
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Destroy(_effectObject);
        }

        if (target != null)
        {
            transform.position = Vector3.Slerp(transform.position, target.transform.position + Vector3.up, Time.deltaTime * 4.0f);
            Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);

            if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
            {
                //effect
                _effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin");

                _effectObject.transform.parent = target.transform;
                _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);


                //object
                if (!target.CompareTag("PLAYER"))
                {
                    ObjStats oStats = target.GetComponent<ObjStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    oStats.nowHealth -= damage + (pStats.basicAttackPower);

                    target = null;
                }

                //player
                if (target.CompareTag("PLAYER"))
                {
                    enemyStats = target.GetComponent<PlayerStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    enemyStats.receviedDamage = (playerId, (damage + (pStats.basicAttackPower)));

                    if (enemyStats.nowHealth <= 0)
                    {
                        pStats.gold += 300;
                    }

                    target = null;
                }
            }
        }
    }
}
