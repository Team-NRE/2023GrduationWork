/// ksPark
/// 
/// 원거리 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using Define;

public class RangeMinion : Minion
{
    [SerializeField]
    GameObject bullet;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Range;

        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}MinionBullet");
    }

    public override void Attack()
    {
        base.Attack();
        GameObject nowBullet = Instantiate(bullet, this.transform.position, this.transform.rotation);
        nowBullet.GetComponent<ObjectBullet>().BulletSetting(this.transform.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }
}
