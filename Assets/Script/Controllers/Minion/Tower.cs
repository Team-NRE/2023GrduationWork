/// ksPark
/// 
/// 타워의 상세 코드 스크립트

using UnityEngine;
using Stat;
using Define;
using Photon.Pun;

public class Tower : ObjectController
{
    LineRenderer lineRenderer;

    string bullet;
    Transform muzzle;

    [Space(10.0f)]
    [Header("- 미니언에게 가하는 최대 체력 비례 공격력")]
    [SerializeField]
    [Range(0.01f, 1.0f)]
    float meleeMinionAttackRatio = 0.45f;
    [SerializeField]
    [Range(0.01f, 1.0f)]
    float rangeMinionAttackRatio = 0.70f;
    [SerializeField]
    [Range(0.01f, 1.0f)]
    float superMinionAttackRatio = 0.14f;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Turret;

        lineRenderer = GetComponent<LineRenderer>();
        bullet = $"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}TowerBullet";
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

        GameObject nowBullet = PhotonNetwork.InstantiateRoomObject(bullet, muzzle.position, this.transform.rotation);

        float damage = 0;

        if (_targetEnemyTransform.tag == "OBJECT")
        {
            ObjectController targetObjController = _targetEnemyTransform.GetComponent<ObjectController>();
            ObjStats targetObjStat = _targetEnemyTransform.GetComponent<ObjStats>();

            /// 미니언들 체력 비례 데미지를 입히기 위한 if문
            if (_targetEnemyTransform.GetComponent<ObjectController>()._type == ObjectType.Melee)
                damage = targetObjStat.maxHealth * meleeMinionAttackRatio;
            else if (_targetEnemyTransform.GetComponent<ObjectController>()._type == ObjectType.Range)
                damage =  targetObjStat.maxHealth * rangeMinionAttackRatio;
            else if (_targetEnemyTransform.GetComponent<ObjectController>()._type == ObjectType.Super)
                damage =  targetObjStat.maxHealth * superMinionAttackRatio;
            else
                damage =  _oStats.basicAttackPower;
        }
        else
        {
            damage = _oStats.basicAttackPower;
        }

        PhotonView bulletPv = nowBullet.GetComponent<PhotonView>();
        bulletPv.RPC("BulletSetting",
            RpcTarget.All,
            this.transform.position, 
            _targetEnemyTransform.position, 
            _oStats.attackSpeed, 
            damage
        );
    }

    public override void Death()
    {
        base.Death();

        if (!PhotonNetwork.IsMasterClient) return;

        PlayerStats[] pStats = FindObjectsOfType<PlayerStats>();

        for (int i=0; i<pStats.Length; i++) {
            if (pStats[i].gameObject.layer != gameObject.layer && Vector3.Distance(pStats[i].transform.position, transform.position) <= _oStats.recognitionRange)
            {
                pStats[i].gold += _oStats.gold;
                pStats[i].experience += _oStats.experience;
            }
        }

        PhotonNetwork.Destroy(this.gameObject);
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

        switch (_action)
        {
            case ObjectAction.Attack:
                break;
            case ObjectAction.Death:
                transform.Find("UI").gameObject.SetActive(false);
                break;
            case ObjectAction.Move:
                break;
            case ObjectAction.Idle:
                break;
        }
    }
}
