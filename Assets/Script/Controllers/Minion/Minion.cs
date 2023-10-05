/// ksPark
/// 
/// 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Define;
using Photon.Pun;

public class Minion : ObjectController
{
    protected float damping = 10.0f;

    /// <summary>상단 길 이정표</summary>
    private Transform[] milestoneUpper;
    /// <summary>하단 길 이정표</summary>
    private Transform[] milestoneLower;

    /// <summary>NavMeshAgent</summart>
    private NavMeshAgent nav;
    /// <summary>이동할 오브젝트 라인</summary>
    public ObjectLine line;
    /// <summary>이동한 인덱스</summary>
    private int lineIdx = 1;

    /// <summary>현재 서 있는 지역</summary>
    public ObjectPosArea area;
    
    public override void init()
    {
        base.init();
        nav = GetComponent<NavMeshAgent>();
        GetMilestoneTransform();
    }

    public override void initOnEnable()
    {
        base.initOnEnable();

        if (!PhotonNetwork.IsMasterClient) return;

        transform.Find("UI").gameObject.SetActive(true);

        if (gameObject.layer == LayerMask.NameToLayer("Human"))
            lineIdx = 1;
        else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
            if (line == ObjectLine.UpperLine)
                lineIdx = milestoneUpper.Length - 2;
            else if (line == ObjectLine.LowerLine)
                lineIdx = milestoneLower.Length - 2;
    }

    private void FixedUpdate() 
    {
        if (_oStats.nowBattery > 0) _oStats.nowBattery -= Time.fixedDeltaTime;

        GetTransformArea();
    }

    public override void Attack()
    {
        base.Attack();
        if (!PhotonNetwork.IsMasterClient) return;
        if (_targetEnemyTransform == null) return;

        var targetRotation = Quaternion.LookRotation(_targetEnemyTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);
    }

    public override void Death()
    {
        base.Death();
        Destroy(this.gameObject);
    }
    public override void Move()
    {
        base.Move();

        if (!PhotonNetwork.IsMasterClient) return;

        Vector3 moveTarget = GetMoveTarget();

        var targetPos = new Vector3(moveTarget.x, transform.position.y, moveTarget.z) - transform.position;
        if (targetPos != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(targetPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);
        }

        nav.SetDestination(moveTarget);
    }

    protected override void UpdateObjectAction()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_oStats.nowHealth <= 0 || _oStats.nowBattery <= 0)
        {
            _action = ObjectAction.Death;
        }
        else if (_targetEnemyTransform != null)
        {
            if (Vector3.Distance(transform.position, _targetEnemyTransform.position) <= _oStats.attackRange)
            {
                _action = ObjectAction.Attack;
            }
            else
            {
                _action = ObjectAction.Move;
            }
        }
        else if (line == ObjectLine.UpperLine && 0 <= lineIdx && lineIdx < milestoneUpper.Length)
        {
            _action = ObjectAction.Move;
        }
        else if (line == ObjectLine.LowerLine && 0 <= lineIdx && lineIdx < milestoneLower.Length)
        {
            _action = ObjectAction.Move;
        }
        else
        {
            _action = ObjectAction.Idle;
        }

        switch (_action)
        {
            case ObjectAction.Attack:
                nav.enabled = false;
                nav.enabled = true;
                break;
            case ObjectAction.Death:
                nav.enabled = false;
                GetComponent<Collider>().enabled = false;
                break;
            case ObjectAction.Move:
                nav.enabled = true;
                break;
            case ObjectAction.Idle:
                nav.enabled = false;
                break;
        }
    }

    private Vector3 GetMoveTarget()
    {
        Vector3 result = Vector3.zero;

        if (_targetEnemyTransform != null && area == ObjectPosArea.Road)
        {
            if (_targetEnemyTransform.CompareTag("PLAYER") && _targetEnemyTransform.GetComponent<BaseController>()._area == ObjectPosArea.Road)
                return _targetEnemyTransform.position;
            if (_targetEnemyTransform.CompareTag("OBJECT"))
                return _targetEnemyTransform.position;
        }

        if (line == ObjectLine.UpperLine) result = milestoneUpper[lineIdx].position;
        if (line == ObjectLine.LowerLine) result = milestoneLower[lineIdx].position;

        if (gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            if (Vector3.Distance(transform.position, result) <= 1f || transform.position.x - result.x > 1.0f)
                lineIdx++;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
        {
            if (Vector3.Distance(transform.position, result) <= 1f || transform.position.x - result.x < 1.0f)
                lineIdx--;
        }

        return result;
    }

    /// <summary>
    /// 이동을 위한 이정표들을 불러오는 함수
    /// </summary>
    private void GetMilestoneTransform()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        milestoneUpper = GameObject.Find("Upper").GetComponentsInChildren<Transform>();
        milestoneLower = GameObject.Find("Lower").GetComponentsInChildren<Transform>();

        milestoneUpper[0] = milestoneLower[0] = GameObject.Find("HumanNexus")?.transform;
        milestoneUpper[milestoneUpper.Length - 1] = milestoneLower[milestoneLower.Length - 1] = GameObject.Find("CyborgNexus")?.transform;
    }

    private void GetTransformArea()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        area = Managers.game.GetPosAreaInMap(transform.position);
    }
}