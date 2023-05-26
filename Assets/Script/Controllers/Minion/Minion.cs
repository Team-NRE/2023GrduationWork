/// ksPark
/// 
/// 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using Define;
using Photon.Pun;
using Photon.Realtime;

public class Minion : ObjectController, IPunObservable
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

        Debug.Log("isSpawn");

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

        // Managers.Pool.Push(poolable);
    }

    public override void Move()
    {
        base.Move();
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;

        Vector3 nowMilestone = Vector3.zero;

        if (line == ObjectLine.UpperLine)
        {
            nowMilestone = milestoneUpper[lineIdx].position;
        }
        else if (line == ObjectLine.LowerLine)
        {
            nowMilestone = milestoneLower[lineIdx].position;
        }

        if (Vector3.Distance(transform.position, nowMilestone) <= 0.2f)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Human"))
                lineIdx++;
            else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
                lineIdx--;
        }

        transform.LookAt(nowMilestone);
        nav.SetDestination(nowMilestone);
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0) _action = ObjectAction.Death;
        else if (_targetEnemyTransform == null) _action = ObjectAction.Move;
        else _action = ObjectAction.Attack;
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