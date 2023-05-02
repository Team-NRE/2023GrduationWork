/// ksPark
/// 
/// 오브젝트 컨트롤 최상위 스크립트
/// (Minion, Tower, Nexus, Neutral Mob)

using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;

[RequireComponent(typeof(ObjStats))]

public abstract class ObjectController : MonoBehaviour
{
    //외부 namespace Stat 참조
    public ObjStats _oStats { get; set; }

    //외부 namespace Define 참조
    public ObjectAction _action { get; set; }
    public ObjectType _type { get; set; }

    //모든 오브젝트의 Transform이 담긴 배열
    public static List<Transform> _allObjectTransforms = new List<Transform>();
    public Transform _targetEnemyTransform;

    //초기화
    protected Animator animator { get; set; }

    public void Awake()
    {
        _allObjectTransforms.Add(transform);
        _oStats = GetComponent<ObjStats>();
        animator = GetComponent<Animator>();

        init();
    }

    /// <summary>
    /// 초기화 함수, 하위 객체에서 초기화용
    /// </summary>
    public virtual void init() { }

    public void Update()
    {
        UpdateInRangeEnemyObjectTransform();
        UpdateObjectAction();
        ExecuteObjectAnim();
    }

    /// <summary>
    /// 오브젝트 상태에 따른 애니메이션 재생
    /// </summary>
    protected void ExecuteObjectAnim()
    {
        switch (_action)
        {
            case ObjectAction.Attack:
                animator.SetBool("Attack", true);
                animator.SetBool("Death", false);
                animator.SetBool("Move", false);
                break;
            case ObjectAction.Death:
                animator.SetBool("Attack", false);
                animator.SetBool("Death", true);
                animator.SetBool("Move", false);
                break;
            case ObjectAction.Move:
                animator.SetBool("Attack", false);
                animator.SetBool("Death", false);
                animator.SetBool("Move", true);
                Move();
                break;
            case ObjectAction.None:
                break;
        }
    }

    /// <summary>
    /// 공격 코드 함수
    /// </summary>
    public virtual void Attack() { }
    /// <summary>
    /// 죽음 코드 함수
    /// </summary>
    public virtual void Death() {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 이동 코드 함수
    /// </summary>
    public virtual void Move() { }

    /// <summary>
    /// 오브젝트 상태 전이 코드
    /// </summary>
    protected virtual void UpdateObjectAction() { }

    /// <summary>
    /// 공격 타겟(가장 가까운 적)을 정하는 스크립트 
    /// </summary>
    protected void UpdateInRangeEnemyObjectTransform()
    {
        Transform newTarget = null;
        float minRange = _oStats.attackRange;

        for (int i=0; i<_allObjectTransforms.Count; i++)
        {
            if (_allObjectTransforms[i].gameObject.layer == gameObject.layer) continue;

            float nowRange = Vector3.Distance(transform.position, _allObjectTransforms[i].position);

            // **라인에 있는 조건도 넣을 것.**
            if (minRange >= nowRange)
            {
                minRange = nowRange;
                newTarget = _allObjectTransforms[i];
            }
        }

        _targetEnemyTransform = newTarget;
    }
}