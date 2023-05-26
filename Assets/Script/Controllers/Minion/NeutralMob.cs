/// ksPark
/// 
/// 중립 몹의 상세 코드 스크립트

using UnityEngine;
using Stat;
using Define;

public class NeutralMob : ObjectController
{
    GameObject bullet;
    public Transform[] muzzles;
    private LineRenderer lineRenderer;

    public float _specialAttackCoolingTime = 10;
    private float _specialAttackCoolingTimeNow;

    private bool isMachineGun;

    public override void init() 
    {
        base.init();

        bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/ObjectBullet");
        lineRenderer = GetComponent<LineRenderer>();
        _specialAttackCoolingTimeNow = _specialAttackCoolingTime;
        isMachineGun = false;

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

            isMachineGun = (Vector3.Distance(transform.position, _targetEnemyTransform.position) < 0.5f * _oStats.attackRange);

            if (isMachineGun) lineRenderer.positionCount = 0;
            else 
            {
                lineRenderer.positionCount = 2;

                lineRenderer.startWidth = .125f;
                lineRenderer.endWidth = .25f;

                lineRenderer.SetPosition(0, muzzles[2].position);
                lineRenderer.SetPosition(1, _targetEnemyTransform.position);
            }
        }
        else 
        {
            lineRenderer.positionCount = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime);
            _action = ObjectAction.Idle;
        }
    }

    private void BasicAttack()
    {
        if (_targetEnemyTransform == null) return;

        if (isMachineGun) MachineGun();
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
        if (_targetEnemyTransform == null) return;

        //타겟이 적 Player일 시
        if (_targetEnemyTransform.tag == "PLAYER")
        {
            PlayerStats _Stats = _targetEnemyTransform.GetComponent<PlayerStats>();
            _Stats.nowHealth -= _oStats.basicAttackPower;
        }
        else
        {
            ObjStats _Stats = _targetEnemyTransform.GetComponent<ObjStats>();
            _Stats.nowHealth -= _oStats.basicAttackPower;
        }
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
