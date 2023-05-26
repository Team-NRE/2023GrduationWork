/// ksPark
/// 
/// 오브젝트 컨트롤 최상위 스크립트
/// (Minion, Tower, Nexus, Neutral Mob)

using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(ObjStats))]

public abstract class ObjectController : MonoBehaviour
{
	//동기화 기점
	private PhotonView _pv;

	//위치 동기화 코드
	private Vector3 receivePos;
	private Quaternion receiveRot;

	//외부 namespace Stat 참조
	public ObjStats _oStats { get; set; }

    //외부 namespace Define 참조
    public ObjectAction _action; //{ get; set; }
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

    public void OnEnable()
    {
        _oStats.InitStatSetting(_type);

        initOnEnable();
    }

    /// <summary>
    /// 초기화 함수, 하위 객체에서 초기화용
    /// </summary>
    public virtual void init() 
    {
        _pv = GetComponent<PhotonView>();
	}

    /// <summary>
    /// 초기화 함수, OnEnable될 때 마다 실행될 코드 작성용
    /// </summary>
    public virtual void initOnEnable(){ }

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
            case ObjectAction.Idle:
                animator.SetBool("Attack", false);
                animator.SetBool("Death", false);
                animator.SetBool("Move", false);
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
    public virtual void Death() { }
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
        float minRange = _oStats.recognitionRange;

        for (int i=0; i<_allObjectTransforms.Count; i++)
        {
            if (_allObjectTransforms[i].gameObject.activeSelf == false) continue; 
            if (_allObjectTransforms[i].gameObject.layer == gameObject.layer) continue;

            if (_allObjectTransforms[i].gameObject.tag == "PLAYER")
            {
                if (_allObjectTransforms[i].GetComponent<PlayerController>()._state == State.Die) continue;
            }
            else
            {
                if (_allObjectTransforms[i].GetComponent<ObjectController>()._action == ObjectAction.Death) continue;
            }

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

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// 자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신 
		if (stream.IsWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			receivePos = (Vector3)stream.ReceiveNext();
			receiveRot = (Quaternion)stream.ReceiveNext();
		}
	}
}