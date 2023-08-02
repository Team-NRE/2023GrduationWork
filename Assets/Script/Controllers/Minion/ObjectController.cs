/// ksPark
/// 
/// 오브젝트 컨트롤 최상위 스크립트
/// (Minion, Tower, Nexus, Neutral Mob)

using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;

using Photon.Pun;

[RequireComponent(typeof(ObjStats))]

public abstract class ObjectController : MonoBehaviour
{
	//위치 동기화 코드
	protected Vector3 receivePos;
	protected Quaternion receiveRot;

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

    //처치시 코인 드랍 파티클
    [SerializeField]
    GameObject particleCoinDrop;

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
        resetMinimapIconColor();

        initOnEnable();
    }

    /// <summary>
    /// 초기화 함수, 하위 객체에서 초기화용
    /// </summary>
    public virtual void init() { }

    /// <summary>
    /// 초기화 함수, OnEnable될 때 마다 실행될 코드 작성용
    /// </summary>
    public virtual void initOnEnable(){ }

    public void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        UpdateInRangeEnemyObjectTransform();
        UpdateObjectAction();
        ExecuteObjectAnim();
    }

    /// <summary>
    /// 오브젝트 상태에 따른 애니메이션 재생
    /// </summary>
    protected void ExecuteObjectAnim()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
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
    public virtual void Attack() {if (!PhotonNetwork.IsMasterClient) return; }
    /// <summary>
    /// 죽음 코드 함수
    /// </summary>
    public virtual void Death() {if (!PhotonNetwork.IsMasterClient) return; }
    /// <summary>
    /// 이동 코드 함수
    /// </summary>
    public virtual void Move() {if (!PhotonNetwork.IsMasterClient) return; }

    /// <summary>
    /// 오브젝트 상태 전이 코드
    /// </summary>
    protected virtual void UpdateObjectAction() {if (!PhotonNetwork.IsMasterClient) return; }

    /// <summary>
    /// 공격 타겟(가장 가까운 적)을 정하는 스크립트 
    /// </summary>
    protected void UpdateInRangeEnemyObjectTransform()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Transform newTargetPlayer = null, newTargetObject = null;
        float minRangePlayer = _oStats.recognitionRange, minRangeObject = _oStats.recognitionRange;

        for (int i=0; i<_allObjectTransforms.Count; i++)
        {
            if (_allObjectTransforms[i].gameObject.activeSelf == false) continue; 
            if (_allObjectTransforms[i].gameObject.layer == gameObject.layer) continue;

            if (_allObjectTransforms[i].gameObject.tag == "PLAYER")
            {
                if (_allObjectTransforms[i].GetComponent<BaseController>()._state == State.Die) continue;
            }
            else
            {
                if (_allObjectTransforms[i].GetComponent<ObjectController>()._action == ObjectAction.Death) continue;
            }

            float nowRange = Vector3.Distance(transform.position, _allObjectTransforms[i].position);

            if (_allObjectTransforms[i].gameObject.tag == "PLAYER")
            {
                if (minRangePlayer >= nowRange)
                {
                    minRangePlayer = nowRange;
                    newTargetPlayer = _allObjectTransforms[i].transform;
                }
            }
            else if (_allObjectTransforms[i].gameObject.tag == "OBJECT")
            {
                if (minRangeObject >= nowRange)
                {
                    minRangeObject = nowRange;
                    newTargetObject = _allObjectTransforms[i].transform;
                }
            }
        }

        _targetEnemyTransform = (newTargetObject != null) ? newTargetObject : newTargetPlayer;
    }

    protected void UpdateInRangeEnemyObjectTransform_OverlapSphere()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Transform newTargetPlayer = null, newTargetObject = null;
        float minRangePlayer = _oStats.recognitionRange, minRangeObject = _oStats.recognitionRange;

        Collider[] inRangeObject = Physics.OverlapSphere(this.transform.position, minRangePlayer);

        for (int i=0; i<inRangeObject.Length; i++)
        {
            if (inRangeObject[i].gameObject.activeSelf == false) continue; 
            if (inRangeObject[i].gameObject.layer == gameObject.layer) continue;

            if (inRangeObject[i].gameObject.tag == "PLAYER")
            {
                if (inRangeObject[i].GetComponent<BaseController>()._state == State.Die) continue;
            }
            else if (inRangeObject[i].gameObject.tag == "OBJECT")
            {
                if (inRangeObject[i].GetComponent<ObjectController>()._action == ObjectAction.Death) continue;
            }
            else
            {
                continue;
            }

            float nowRange = Vector3.Distance(transform.position, inRangeObject[i].transform.position);

            if (inRangeObject[i].gameObject.tag == "PLAYER")
            {
                if (minRangePlayer >= nowRange)
                {
                    minRangePlayer = nowRange;
                    newTargetPlayer = inRangeObject[i].transform;
                }
            }
            else if (inRangeObject[i].gameObject.tag == "OBJECT")
            {
                if (minRangeObject >= nowRange)
                {
                    minRangeObject = nowRange;
                    newTargetObject = inRangeObject[i].transform;
                }
            }
        }

        _targetEnemyTransform = (newTargetObject != null) ? newTargetObject : newTargetPlayer;
    }

    private void resetMinimapIconColor()
    {
        SpriteRenderer icon = gameObject.GetComponentInChildren<SpriteRenderer>();

        if (icon == null) return;

        if (gameObject.layer == ((int)Layer.Cyborg))
            icon.color = Color.red;
        else if (gameObject.layer == ((int)Layer.Human))
            icon.color = Color.blue;
    }

    protected void summonCoinDrop()
    {
        GameObject coinDrop = Instantiate(particleCoinDrop, transform.position, transform.rotation);
        coinDrop.GetComponent<Particle_CoinDrop>().setInit(_oStats.gold.ToString());
    }
}