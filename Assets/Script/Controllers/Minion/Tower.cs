/// ksPark
/// 
/// 타워의 상세 코드 스크립트

using UnityEngine;
using Define;
using Photon.Pun;

public class Tower : ObjectController
{
    GameObject bullet;
    Transform muzzle;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Turret;

        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/ObjectBullet");
        muzzle = transform.Find("BulletPos");
    }

    public override void Attack()
    {
        base.Attack();

        if (_targetEnemyTransform == null) return;

        GameObject nowBullet = PhotonNetwork.Instantiate($"Prefabs/Projectile/ObjectBullet", this.transform.position, this.transform.rotation);
        nowBullet.GetComponent<ObjectBullet>().BulletSetting(muzzle.position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
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
