using UnityEngine;
using Stat;
using Photon.Pun;

public class SpearStart : BaseEffect
{
    float bulletSpeed;
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;
    int _playerId;

    Transform playerTr;
    PlayerStats pStats;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
        pStats = player.GetComponent<PlayerStats>();
        
        bulletSpeed = 30.0f;
        damage = 15.0f;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);
        this.gameObject.transform.parent = null;
    }

    public void Update()
    {
        FollowTarget(_playerId);
    }

    [PunRPC]
    public void FollowTarget(int userId) 
    {
        Vector3 SpearDirection = player.transform.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        if (other.gameObject.layer == enemylayer)
        {
            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (_playerId, damage + (pStats.basicAttackPower * 0.7f));
            }

            //PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
