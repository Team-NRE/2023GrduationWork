/// ksPark
/// 
/// 중립 몹의 상세 코드 스크립트

using UnityEngine;
using Define;

public class NeutralMob : ObjectController
{
    GameObject bullet;
    public Transform[] muzzles;
    private LineRenderer lineRenderer;

    public float _specialAttackCoolingTime = 10;
    private float _specialAttackCoolingTimeNow;

    public override void init() 
    {
        base.init();

        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/ObjectBullet");
        lineRenderer = GetComponent<LineRenderer>();
        _specialAttackCoolingTimeNow = _specialAttackCoolingTime;

        _type = ObjectType.Neutral;
    }

    private void FixedUpdate() {
        if (_action == ObjectAction.Attack && _specialAttackCoolingTimeNow > 0)
            _specialAttackCoolingTimeNow -= Time.deltaTime;
    }

    public override void Attack()
    {
        base.Attack();

        if (_specialAttackCoolingTimeNow > 0) BasicAttack();
        else SpecialAttack();
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void UpdateObjectAction()
    {
        if (_targetEnemyTransform != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetEnemyTransform.position - transform.position), Time.deltaTime * 2.0f);
            _action = ObjectAction.Attack;
        }
        else 
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime);
            _action = ObjectAction.Idle;
            
        }
    }

    private void BasicAttack()
    {
        if (Vector3.Distance(transform.position, _targetEnemyTransform.position) < 0.5f * _oStats.attackRange) MachineGun();
        else Laser();
    }

    private void SpecialAttack()
    {
        int type = Random.Range(0, 2);

        switch (type)
        {
            case 0:
                Missile();
                break;
            case 1:
                EnergyRelease();
                break;
            case 2:
                ProtectiveShield();
                break;
        }

        _specialAttackCoolingTimeNow = _specialAttackCoolingTime;
    }

    #region 공격 함수
    private void MachineGun()
    {
        Managers.Pool.Pop(bullet).Proj_Target_Init(
            muzzles[0].position, _targetEnemyTransform, 
            _oStats.attackSpeed, _oStats.basicAttackPower);

        Managers.Pool.Pop(bullet).Proj_Target_Init(
            muzzles[1].position, _targetEnemyTransform, 
            _oStats.attackSpeed, _oStats.basicAttackPower);
    }

    private void Laser()
    {
        lineRenderer.startWidth = .125f;
        lineRenderer.endWidth = .25f;

        lineRenderer.SetPosition(0, muzzles[2].position);
        lineRenderer.SetPosition(1, _targetEnemyTransform.position);
    }

    private void Missile()
    {

    }

    private void EnergyRelease()
    {

    }

    private void ProtectiveShield()
    {

    }
    #endregion
}
