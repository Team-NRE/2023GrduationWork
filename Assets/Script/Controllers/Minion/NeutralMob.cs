/// ksPark
/// 
/// 중립 몹의 상세 코드 스크립트

using UnityEngine;
using Stat;
using Define;
using Photon.Pun;

public class NeutralMob : ObjectController
{
    [Header ("- Basic Attack")]
    GameObject bullet;
    public Transform[] muzzles;
    private LineRenderer lineRenderer;

    [Header ("- Special Attack")]
    public GameObject missile;
    public GameObject EnergyRelease;

    public float _specialAttackCoolingTime = 10;
    public float _specialAttackCoolingTimeNow;

    private bool isMachineGun;

    public override void init() 
    {
        base.init();
        _pv = GetComponent<PhotonView>();
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
        Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }

    protected override void UpdateObjectAction()
    {
        if (_targetEnemyTransform != null)
        {
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(_targetEnemyTransform.position - this.transform.position), Time.deltaTime * 2.0f);
            _action = ObjectAction.Attack;

            isMachineGun = (Vector3.Distance(this.transform.position, _targetEnemyTransform.position) < 0.5f * _oStats.attackRange);

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

            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0), Time.deltaTime);
            _action = ObjectAction.Idle;
        }
    }

    [PunRPC]
    private void BasicAttack()
    {
        if (_targetEnemyTransform == null) return;

        if (isMachineGun) MachineGun();
        else Laser();
    }

    [PunRPC]
    private void SpecialAttack()
    {
        if (_targetEnemyTransform == null) return;

        int type = Random.Range(0, 2);

        switch (type)
        {
            case 0:
                animator.SetTrigger("Missile");
                break;
            case 1:
                animator.SetTrigger("Energy");
                break;
        }

        _specialAttackCoolingTimeNow = _specialAttackCoolingTime;
    }

    #region 공격 함수
    private void MachineGun()
    {
        GameObject nowBullet = PhotonNetwork.Instantiate($"Prefabs/Projectile/ObjectBullet", this.transform.position, this.transform.rotation);
        nowBullet.GetComponent<ObjectBullet>().BulletSetting(muzzles[0].position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);

        nowBullet = PhotonNetwork.Instantiate($"Prefabs/Projectile/ObjectBullet", this.transform.position, this.transform.rotation);
        nowBullet.GetComponent<ObjectBullet>().BulletSetting(muzzles[1].position, _targetEnemyTransform, _oStats.attackSpeed, _oStats.basicAttackPower);
    }

    private void Laser()
    {
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
        GameObject SummonedMissile = Instantiate(missile);
        SummonedMissile.GetComponent<Missile>().SummonMissile(_allObjectTransforms, _targetEnemyTransform.position, _oStats.basicAttackPower, 3.0f);
    }

    private void Energy()
    {
        GameObject SummonedEnergyRelease = Instantiate(EnergyRelease);
        SummonedEnergyRelease.GetComponent<EnergyRelease>().SummonEnergyRelease(_allObjectTransforms, transform.position, _oStats.basicAttackPower, 5.0f);
    }
}
#endregion