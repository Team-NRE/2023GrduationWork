/// ksPark
/// 
/// 원거리 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using Define;
using Photon.Pun;

public class RangeMinion : Minion
{
    GameObject bullet;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Range;
    }

    public override void Attack()
    {
        base.Attack();
        //GameObject nowBullet = PhotonNetwork.Instantiate($"Prefabs/Projectile/ObjectBullet", this.transform.position, this.transform.rotation);
        GameObject nowBullet = Managers.Resource.Instantiate($"Projectile/ObjectBullet");
        nowBullet.GetComponent<ObjectBullet>().BulletSetting(this.transform.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }
}
