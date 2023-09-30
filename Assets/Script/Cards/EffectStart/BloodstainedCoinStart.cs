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
        //BaseCard._lockTarget = null;
        damage = 10.0f;
        _playerId = userId;
        _targetId = targetId;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.8f, 0);
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
                // 여기서 에러가 발생한다.
                //effect
                _effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin");
                
                //_effectObject = PhotonNetwork.Instantiate($"Particle/Effect_BloodstainedCoin", this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                _effectObject.transform.parent = target.transform;
                _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);


                //Ÿ���� �̴Ͼ�, Ÿ���� �� 
                if (target.tag != "PLAYER")
                {
                    ObjStats oStats = target.GetComponent<ObjStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    oStats.nowHealth -= damage + (pStats.basicAttackPower);

                    //Destroy(gameObject, 0.1f);
                    //Destroy(_effectObject, 0.5f);
                    target = null;
                    //RpcDelayDestroy(this.gameObject.GetComponent<PhotonView>().ViewID, 0.1f);
                    //RpcDelayDestroy(_effectObject.GetComponent<PhotonView>().ViewID, 0.5f);
                }

                //Ÿ���� �� Player�� ��
                else if (target.tag == "PLAYER")
                {
                    enemyStats = target.GetComponent<PlayerStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    enemyStats.receviedDamage = (playerId, (damage + (pStats.basicAttackPower)));

                    if (enemyStats.nowHealth <= 0)
                    {
                        pStats.kill += 1;
                        pStats.gold += 100;
                    }

                    target = null;
                    //Destroy(gameObject, 0.1f);
                    //Destroy(_effectObject, 0.5f);
                    //RpcDelayDestroy(this.gameObject.GetComponent<PhotonView>().ViewID, 0.1f);
                    //RpcDelayDestroy(_effectObject.gameObject.GetComponent<PhotonView>().ViewID, 0.5f);
                    //StartCoroutine(DelayDestroy(gameObject, 0.1f));
                }
            }
        }
    }
}
