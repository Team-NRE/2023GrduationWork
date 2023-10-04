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
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        //playerTr = player.transform;
        bulletSpeed = 30.0f;
        damage = 15.0f;
        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
        pStats = player.GetComponent<PlayerStats>();
        _playerId = userId;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);
        this.gameObject.transform.parent = null;
    }

    public void Update()
    {
        FollowTarget(_playerId);
        //_pv.RPC("FollowTarget", RpcTarget.All, _playerId);
    }

    [PunRPC]
    public void FollowTarget(int userId) 
    {
        GameObject user = Managers.game.RemoteTargetFinder(userId);
        //Vector3 SpearDirection = playerTr.forward;
        Vector3 SpearDirection = user.transform.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        //int id = Managers.game.RemoteColliderId(other);
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == enemylayer)
        {
            //Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
                Debug.Log(oStats.nowHealth);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (_playerId, damage + (pStats.basicAttackPower * 0.7f));
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
                Debug.Log(enemyStats.nowHealth);
            }

            //PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
