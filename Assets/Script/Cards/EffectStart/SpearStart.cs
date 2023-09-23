using UnityEngine;
using Stat;
using Photon.Pun;

public class SpearStart : BaseEffect
{
    float bulletSpeed;
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;

    Transform playerTr;
    PlayerStats pStats;

    public void StartSpear(int player, int _enemylayer, float _damage)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(player);

        pStats = _player.GetComponent<PlayerStats>();
        playerTr = _player.transform;

        enemylayer = _enemylayer;
        bulletSpeed = 30f; // 공속 대비 5배 속도
        damage = _damage;
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        playerTr = player.transform;
        bulletSpeed = 30.0f;
        damage = 15.0f;
    }

    public void Update()
    {
        //FollowTarget();
        _pv.RPC("FollowTarget", RpcTarget.All);
    }

    [PunRPC]
    public void FollowTarget() 
    {
        Vector3 SpearDirection = playerTr.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    public void OnTriggerEnter(Collider other)
    {
        int id = Managers.game.RemoteTargetIdFinder(other.gameObject);
        //int id = Managers.game.RemoteColliderId(other);
        _pv.RPC("RpcTrigger", RpcTarget.All, id);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

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

                enemyStats.receviedDamage = damage + (pStats.basicAttackPower * 0.7f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
