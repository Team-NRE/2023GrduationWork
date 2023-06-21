/// ksPark
/// 
/// 타워의 상세 코드 스크립트

using UnityEngine;
using Define;
using Photon.Pun;

public class Tower : ObjectController
{
    LineRenderer lineRenderer;

    GameObject bullet;
    Transform muzzle;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Turret;

        lineRenderer = GetComponent<LineRenderer>();
        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}TowerBullet");
        muzzle = transform.Find("BulletPos");
    }

    private void FixedUpdate()
    {
        if (lineRenderer != null)
        {
            if (_targetEnemyTransform == null)
            {
                lineRenderer.positionCount = 0;
            }
            else
            {
                lineRenderer.positionCount = 2;

                lineRenderer.startWidth = .04f;
                lineRenderer.endWidth = .04f;

                lineRenderer.SetPosition(0, muzzle.position);
                lineRenderer.SetPosition(1, _targetEnemyTransform.position);
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (_targetEnemyTransform == null) return;

        Debug.Log(bullet);
        Debug.Log(muzzle.position);
        Debug.Log(this.transform.rotation);
        GameObject nowBullet = Instantiate(bullet, muzzle.position, this.transform.rotation);
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
            transform.Find("UI").gameObject.SetActive(false);
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
