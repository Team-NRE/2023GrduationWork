/// ksPark
/// 
/// 원거리 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using Define;

using Photon.Pun;

public class RangeMinion : Minion
{
    [SerializeField]
    string bullet;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Range;

        bullet = $"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}MinionBullet";
    }

    public override void Attack()
    {
        base.Attack();
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("this code running");

        GameObject nowBullet = PhotonNetwork.InstantiateRoomObject(bullet, this.transform.position, this.transform.rotation);
        PhotonView bulletPv = nowBullet.GetComponent<PhotonView>();
        bulletPv.RPC("BulletSetting",
            RpcTarget.All,
            this.transform.position, 
            _targetEnemyTransform.position, 
            _oStats.attackSpeed, 
            _oStats.basicAttackPower
        );
        // nowBullet.GetComponent<PhotonView>().BulletSetting(this.transform.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }
}
