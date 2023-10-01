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
    string bullet;
    public Transform[] muzzles;
    [SerializeField]
    private LineRenderer lineRenderer;

    [Header ("- Special Attack")]
    public string missile;
    public string energyRelease;

    public float _specialAttackCoolingTime = 10;
    public float _specialAttackCoolingTimeNow;

    private bool isMachineGun;

    public override void init() 
    {
        base.init();
        bullet =        $"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}MobBullet";
        missile =       $"Prefabs/Projectile/Missile";
        energyRelease = $"Prefabs/Projectile/EnergyRelease";
        lineRenderer = GetComponent<LineRenderer>();
        _specialAttackCoolingTimeNow = _specialAttackCoolingTime;
        isMachineGun = false;

        _type = ObjectType.Neutral;
    }

    private void FixedUpdate() {
        if (lineRenderer != null)
        {
            if (_targetEnemyTransform != null && 
                Vector3.Distance(transform.position, _targetEnemyTransform.position) <= _oStats.attackRange &&
                Vector3.Distance(transform.position, _targetEnemyTransform.position) > 0.5f * _oStats.attackRange)
            {
                lineRenderer.positionCount = 2;

                lineRenderer.startWidth = .125f;
                lineRenderer.endWidth = .25f;

                lineRenderer.SetPosition(0, muzzles[2].position);
                lineRenderer.SetPosition(1, _targetEnemyTransform.position);
            } 
            else
            {
                lineRenderer.positionCount = 0;
                _targetEnemyTransform = null;
            }
        }

        if (!PhotonNetwork.IsMasterClient) return;

        if (_action == ObjectAction.Attack && _specialAttackCoolingTimeNow > 0)
            _specialAttackCoolingTimeNow -= Time.deltaTime;
    }

    public override void Attack()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        base.Attack();

        if (_specialAttackCoolingTimeNow > 0) BasicAttack();
        else SpecialAttack();
    }

    public override void Death()
    {
        base.Death();
        Debug.Log("asdf");

        float minDistance = float.MaxValue;
        Layer team = Layer.Human;
        if (Managers.game.humanTeamCharacter.Item1 != null && minDistance > Vector3.Distance(transform.position, Managers.game.humanTeamCharacter.Item1.transform.position))
        {
            minDistance = Vector3.Distance(transform.position, Managers.game.humanTeamCharacter.Item1.transform.position);
            team = Layer.Human;
        }
        if (Managers.game.humanTeamCharacter.Item2 != null && minDistance > Vector3.Distance(transform.position, Managers.game.humanTeamCharacter.Item2.transform.position))
        {
            minDistance = Vector3.Distance(transform.position, Managers.game.humanTeamCharacter.Item2.transform.position);
            team = Layer.Human;
        }
        if (Managers.game.cyborgTeamCharacter.Item1 != null && minDistance > Vector3.Distance(transform.position, Managers.game.cyborgTeamCharacter.Item1.transform.position))
        {
            minDistance = Vector3.Distance(transform.position, Managers.game.cyborgTeamCharacter.Item1.transform.position);
            team = Layer.Cyborg;
        }
        if (Managers.game.cyborgTeamCharacter.Item2 != null && minDistance > Vector3.Distance(transform.position, Managers.game.cyborgTeamCharacter.Item2.transform.position))
        {
            minDistance = Vector3.Distance(transform.position, Managers.game.cyborgTeamCharacter.Item2.transform.position);
            team = Layer.Cyborg;
        }

        if ((int)team == Managers.game.myCharacter.layer)
        {
            string cardName = (Random.Range(0, 2) == 0 ? "SpecialCard_EnergyAmp" : "SpecialCard_MissileBomb");
            BaseCard._initDeck.Add(cardName);
            BaseCard._MyDeck  .Add(cardName);
        }

        _allObjectTransforms.Remove(this.transform);

        Destroy(this.gameObject);
    }

    protected override void UpdateObjectAction()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_oStats.nowHealth <= 0) {
                _action = ObjectAction.Death;
                transform.Find("Model/UI").gameObject.SetActive(false);
            }
            else if (_targetEnemyTransform != null)
            {
                transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_targetEnemyTransform.position - this.transform.position), Time.deltaTime * 2.0f);
                _action = ObjectAction.Attack;

                isMachineGun = (Vector3.Distance(this.transform.position, _targetEnemyTransform.position) < 0.5f * _oStats.attackRange);
            }
            else 
            {
                transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0), Time.deltaTime);
                _action = ObjectAction.Idle;
            }   
        }

        switch (_action)
        {
            case ObjectAction.Attack:
                break;
            case ObjectAction.Death:
                GetComponent<Collider>().enabled = false;
                break;
            case ObjectAction.Move:
                break;
            case ObjectAction.Idle:
                break;
        }
    }

    private void BasicAttack()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_targetEnemyTransform == null) return;

        if (isMachineGun) MachineGun();
        else 
            pv.RPC(
                "Laser",
                RpcTarget.All,
                _targetEnemyTransform.GetComponent<PhotonView>().ViewID
            );
    }

    private void SpecialAttack()
    {
        if (!PhotonNetwork.IsMasterClient) return;

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
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject nowBullet = PhotonNetwork.Instantiate(bullet, this.transform.position, this.transform.rotation);
        PhotonView bulletPv = nowBullet.GetComponent<PhotonView>();

        bulletPv.RPC("BulletSetting", // v2
            RpcTarget.All,
            GetComponent<PhotonView>().ViewID, 
            _targetEnemyTransform.GetComponent<PhotonView>().ViewID, 
            _oStats.attackSpeed, 
            _oStats.basicAttackPower
        );

        nowBullet = PhotonNetwork.Instantiate(bullet, this.transform.position, this.transform.rotation);
        bulletPv = nowBullet.GetComponent<PhotonView>();
        bulletPv.RPC("BulletSetting",
            RpcTarget.All,
            GetComponent<PhotonView>().ViewID, 
            _targetEnemyTransform.GetComponent<PhotonView>().ViewID, 
            _oStats.attackSpeed, 
            _oStats.basicAttackPower
        );
    }

    [PunRPC]
    private void Laser(int targetId)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _targetEnemyTransform = PhotonView.Find(targetId).transform;  
        }

        //타겟이 적 Player일 시
        if (_targetEnemyTransform.tag == "PLAYER")
        {
            _targetEnemyTransform.GetComponent<PlayerStats>().receviedDamage = (pv.ViewID, _oStats.basicAttackPower);
        }
        else
        {
            _targetEnemyTransform.GetComponent<ObjStats>().nowHealth -= _oStats.basicAttackPower;
        }
    }

    private void Missile()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject SummonedMissile = PhotonNetwork.InstantiateRoomObject(missile, this.transform.position, this.transform.rotation);
        PhotonView missilePv = SummonedMissile.GetComponent<PhotonView>();
        missilePv.RPC("SummonMissile",
            RpcTarget.All,
            pv.ViewID,
            _targetEnemyTransform.GetComponent<PhotonView>().ViewID,
            3.0f
        );
    }

    private void Energy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject SummonedEnergyRelease = PhotonNetwork.InstantiateRoomObject(energyRelease, this.transform.position, this.transform.rotation);
        PhotonView energyReleasePv = SummonedEnergyRelease.GetComponent<PhotonView>();
        energyReleasePv.RPC("SummonEnergyRelease",
            RpcTarget.All,
            pv.ViewID,
            3.0f
        );
    }
}
#endregion