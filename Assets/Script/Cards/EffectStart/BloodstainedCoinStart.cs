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

    Transform target = null;

    float damage = default;

    PlayerStats enemyStats;

    public void StartBloodstainedCoin(int playerId, float _damage)
    {
        //player = GameObject.Find(_player);
        _playerId = playerId;
        player = Managers.game.RemoteTargetFinder(playerId);
        target = BaseCard._lockTarget.transform;
        BaseCard._lockTarget = null;

        damage = _damage;
    }

    public override void CardEffectInit(int userId, int targetId)
    {
        base.CardEffectInit(userId);
        
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (target == null)
        {
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Destroy(_effectObject);
        }

        if (target != null)
        {
            transform.position = Vector3.Slerp(transform.position, target.position + Vector3.up, Time.deltaTime * 4.0f);
            Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);

            if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
            {
                //effect
                //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin");
                _effectObject = PhotonNetwork.Instantiate($"Particle/Effect_BloodstainedCoin", this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                _effectObject.transform.parent = target.transform;
                _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);


                //Ÿ���� �̴Ͼ�, Ÿ���� �� 
                if (target.tag != "PLAYER")
                {
                    ObjStats oStats = target.GetComponent<ObjStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    oStats.nowHealth -= damage + (pStats.basicAttackPower);

                    Destroy(gameObject, 0.1f);
                    Destroy(_effectObject, 0.5f);
                    //_pv.RPC("RpcDelayDestroy", RpcTarget.All, this.gameObject, 0.1f);
                    //DelayDestroy(_effectObject, 0.5f, );
                }

                //Ÿ���� �� Player�� ��
                if (target.tag == "PLAYER")
                {
                    enemyStats = target.GetComponent<PlayerStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    enemyStats.receviedDamage = (damage + (pStats.basicAttackPower));

                    if (enemyStats.nowHealth <= 0)
                    {
                        pStats.kill += 1;
                        pStats.gold += 100;
                    }

                    Destroy(gameObject, 0.1f);
                    Destroy(_effectObject, 0.5f);
                    //StartCoroutine(DelayDestroy(gameObject, 0.1f));
                }
            }
        }
    }
}
