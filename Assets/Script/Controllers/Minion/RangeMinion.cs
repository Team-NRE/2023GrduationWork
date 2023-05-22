/// ksPark
/// 
/// 원거리 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using Define;
using Stat;

public class RangeMinion : Minion
{
    GameObject bullet;

    public override void init() 
    {
        base.init();
        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/ObjectBullet");
        _type = ObjectType.Range;
    }

    public override void Attack()
    {
        base.Attack();
        Managers.Pool.Pop(bullet).Proj_Target_Init(transform.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }
}