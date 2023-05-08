/// ksPark
/// 
/// 타워의 상세 코드 스크립트

using UnityEngine;
using Define;

public class Tower : ObjectController
{
    GameObject bullet;
    Transform muzzle;

    public override void init() 
    {
        base.init();
        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/ObjectBullet");
        muzzle = transform.Find("Crystal/BulletPos");

        //새로운 풀링 해주기
        if (Managers.Pool.GetOriginal(bullet.name) == null) { Managers.Pool.CreatePool(bullet, 5); }
    }

    public override void Attack()
    {
        base.Attack();

        if (_targetEnemyTransform == null) return;

        Managers.Pool.Pop(bullet).Proj_Target_Init(muzzle.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }

    public override void Death()
    {
        base.Death();

        gameObject.SetActive(false);
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0) 
        {
            _action = ObjectAction.Death;
        }
        else if (_targetEnemyTransform != null && Vector3.Distance(transform.position, _targetEnemyTransform.position) <= _oStats.attackRange) 
        {
            _action = ObjectAction.Attack;
        }
        else
        {
            _action = ObjectAction.Idle;
        }
    }
}
