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
    public ObjectAction _action { get; set; }
    public ObjectType _type { get; set; }

    //모든 오브젝트의 Transform이 담긴 배열
    public static List<GameObject> _allObjectTransforms = new List<GameObject>();
    public Transform _targetEnemyTransform;
    Collider[] inRangeObject = new Collider[10];

    //초기화
    protected Animator animator { get; set; }
    protected PhotonView pv { get; set; }

    public void Awake()
    {
        _allObjectTransforms.Add(gameObject);
        _oStats = GetComponent<ObjStats>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();

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
        
        UpdateInRangeEnemyObjectTransform_OverlapSphereNonAlloc();
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
    public virtual void Death() 
    {
        _allObjectTransforms.Remove(this.gameObject);
        
        if (_oStats.gold == 0 && _oStats.experience == 0) return;

        checkGoldAndExperienceRange(Managers.game.humanTeamCharacter.Item1);
        checkGoldAndExperienceRange(Managers.game.humanTeamCharacter.Item2);
        checkGoldAndExperienceRange(Managers.game.cyborgTeamCharacter.Item1);
        checkGoldAndExperienceRange(Managers.game.cyborgTeamCharacter.Item2);
    }
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
            if (_allObjectTransforms[i].activeSelf.Equals(false))       continue;
            if (_allObjectTransforms[i].layer.Equals(gameObject.layer)) continue;

            float nowRange = Vector3.Distance(transform.position, _allObjectTransforms[i].transform.position);

            if (_allObjectTransforms[i].CompareTag("PLAYER"))
            {
                if (_allObjectTransforms[i].GetComponent<BaseController>()._state == State.Die) continue;
                if (minRangePlayer >= nowRange)
                {
                    minRangePlayer = nowRange;
                    newTargetPlayer = _allObjectTransforms[i].transform;
                }
            }
            else if (_allObjectTransforms[i].CompareTag("OBJECT"))
            {
                if (_allObjectTransforms[i].GetComponent<ObjectController>()._action == ObjectAction.Death) continue;
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

        inRangeObject = Physics.OverlapSphere(this.transform.position, minRangePlayer);

        for (int i=0; i<inRangeObject.Length; i++)
        {
            if (inRangeObject[i].gameObject.activeSelf == false) continue; 
            if (inRangeObject[i].gameObject.layer == gameObject.layer) continue;

            float nowRange = Vector3.Distance(transform.position, inRangeObject[i].transform.position);

            if (inRangeObject[i].CompareTag("PLAYER"))
            {
                BaseController nowBaseController;
                inRangeObject[i].TryGetComponent<BaseController>(out nowBaseController);
                if (nowBaseController._state == State.Die) continue;
                if (minRangePlayer >= nowRange)
                {
                    minRangePlayer = nowRange;
                    newTargetPlayer = inRangeObject[i].transform;
                }
            }
            else if (inRangeObject[i].CompareTag("OBJECT"))
            {
                ObjectController nowObjectController;
                inRangeObject[i].TryGetComponent<ObjectController>(out nowObjectController);
                if (nowObjectController._action == ObjectAction.Death) continue;
                if (minRangeObject >= nowRange)
                {
                    minRangeObject = nowRange;
                    newTargetObject = inRangeObject[i].transform;
                }
            }
        }

        _targetEnemyTransform = (newTargetObject != null) ? newTargetObject : newTargetPlayer;
    }

    protected void UpdateInRangeEnemyObjectTransform_OverlapSphereNonAlloc()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Transform newTargetPlayer = null, newTargetObject = null;
        float minRangePlayer = _oStats.recognitionRange;
        float minRangeObject = _oStats.recognitionRange;

        Physics.OverlapSphereNonAlloc(
            this.transform.position,
            minRangePlayer,
            inRangeObject,
            LayerMask.GetMask("Human", "Cyborg", "Neutral") ^ (1 << gameObject.layer)
        );

        for (int i = 0; i < inRangeObject.Length; i++)
        {
            if (inRangeObject[i] == default) break;

            float nowRange = Vector3.Distance(transform.position, inRangeObject[i].transform.position);

            if (inRangeObject[i].CompareTag("PLAYER"))  // 플레이어 처리
            {
                BaseController nowBaseController;
                inRangeObject[i].TryGetComponent<BaseController>(out nowBaseController);
                if (nowBaseController._state == State.Die) continue;
                if (minRangePlayer >= nowRange)
                {
                    minRangePlayer = nowRange;
                    newTargetPlayer = inRangeObject[i].transform;
                }
            }
            else if (inRangeObject[i].CompareTag("OBJECT"))  // 오브젝트 처리
            {
                ObjectController nowObjectController;
                inRangeObject[i].TryGetComponent<ObjectController>(out nowObjectController);
                if (nowObjectController._action == ObjectAction.Death) continue;
                if (minRangeObject >= nowRange)
                {
                    minRangeObject = nowRange;
                    newTargetObject = inRangeObject[i].transform;
                }
            }
        }

        // 범위 내 오브젝트 먼저 공격 처리
        _targetEnemyTransform = (newTargetObject != null) ? newTargetObject : newTargetPlayer;
    }

    /// <summary>
    /// 초기 아이콘 색깔 지정 함수
    /// </summary>
    private void resetMinimapIconColor()
    {
        SpriteRenderer icon = gameObject.GetComponentInChildren<SpriteRenderer>();

        if (icon == null) return;

        if (gameObject.layer == ((int)Layer.Cyborg))
            icon.color = Color.red;
        else if (gameObject.layer == ((int)Layer.Human))
            icon.color = Color.blue;
    }

    /// <summary>
    /// 죽음 시 사거리 내 플레이어 감지
    /// 골드 및 경험치 RPC 처리
    /// </summary>
    /// <param name="playerPv">플레이어의 PhotonView</param>
    private void checkGoldAndExperienceRange(PhotonView playerPv)
    {
        if (playerPv == null) return;
        if (!PhotonNetwork.IsMasterClient) return;

        if (playerPv.gameObject.layer != gameObject.layer && 
            Vector3.Distance(playerPv.transform.position, transform.position) <= _oStats.recognitionRange)
        {
            PhotonView.Get(GameObject.Find("GameScene")).RPC(
                "addGnE",
                RpcTarget.All,
                playerPv.ViewID,
                transform.position,
                _oStats.gold,
                _oStats.experience
            );
        }
    }
}