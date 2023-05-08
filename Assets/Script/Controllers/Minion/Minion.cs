/// ksPark
/// 
/// 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using Define;

public class Minion : ObjectController
{
    /// <summary>상단 길 이정표</summary>
    public Transform[] milestoneUpper;
    /// <summary>하단 길 이정표</summary>
    public Transform[] milestoneLower;

    /// <summary>NavMeshAgent</summart>
    private NavMeshAgent nav;
    /// <summary>이동할 오브젝트 라인</summary>
    public ObjectLine line;
    /// <summary>이동한 인덱스</summary>
    public int lineIdx = 1;

    public override void init()
    {
        base.init();
        nav = GetComponent<NavMeshAgent>();

        _type = ObjectType.Range;
        GetMilestoneTransform();

        if (gameObject.layer == LayerMask.NameToLayer("Human"))
            lineIdx = 1;
        else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
            if (line == ObjectLine.UpperLine)
                lineIdx = milestoneUpper.Length - 2;
            else if (line == ObjectLine.LowerLine)
                lineIdx = milestoneLower.Length - 2;
    }

    public override void Attack()
    {
        base.Attack();
        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.velocity = Vector3.zero;
        transform.LookAt(_targetEnemyTransform);

        if (_targetEnemyTransform == null) return;
    }

    public override void Death()
    {
        base.Death();
        nav.isStopped = true;
        nav.updatePosition = false;
        nav.updateRotation = false;
        nav.velocity = Vector3.zero;
        nav.enabled = false;

        Managers.Pool.Push(GetComponent<Poolable>());
    }

    public override void Move()
    {
        base.Move();
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;

        Vector3 moveTarget = Vector3.zero;

        if (_targetEnemyTransform == null)
        {
            if (line == ObjectLine.UpperLine)
            {
                moveTarget = milestoneUpper[lineIdx].position;
            }
            else if (line == ObjectLine.LowerLine)
            {
                moveTarget = milestoneLower[lineIdx].position;
            }

            if (Vector3.Distance(transform.position, moveTarget) <= 0.2f)
            {
                if (gameObject.layer == LayerMask.NameToLayer("Human"))
                    lineIdx++;
                else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
                    lineIdx--;
            }
        }
        else
        {
            moveTarget = _targetEnemyTransform.position;
        }
        
        transform.LookAt(new Vector3 (
            moveTarget.x,
            transform.position.y,
            moveTarget.z
        ));
        nav.SetDestination(moveTarget);
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0)
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
    }

    /// <summary>
    /// 이동을 위한 이정표들을 불러오는 함수
    /// </summary>
    private void GetMilestoneTransform()
    {
        milestoneUpper = GameObject.Find("Upper").GetComponentsInChildren<Transform>();
        milestoneLower = GameObject.Find("Lower").GetComponentsInChildren<Transform>();

        milestoneUpper[0] = milestoneLower[0] = GameObject.Find("HumanNexus")?.transform;
        milestoneUpper[milestoneUpper.Length - 1] = milestoneLower[milestoneLower.Length - 1] = GameObject.Find("CyborgNexus")?.transform;
    }
}