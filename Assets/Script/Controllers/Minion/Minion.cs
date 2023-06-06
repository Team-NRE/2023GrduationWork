/// ksPark
/// 
/// 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Define;
using Photon.Pun;

public class Minion : ObjectController, IPunObservable
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

    /// <summary>타일 그리드</summary>
    public GridLayout grid;
    /// <summary>타일 맵</summary>
    public Tilemap tilemap;
    /// <summary>현재 서 있는 지역</summary>
    public ObjectPosArea area;
    
    public override void init()
    {
        base.init();
        //_pv = GetComponent<PhotonView>();
        grid = FindObjectOfType<GridLayout>();
        tilemap = FindObjectOfType<Tilemap>();
        nav = GetComponent<NavMeshAgent>();
        GetMilestoneTransform();
    }

    public override void initOnEnable()
    {
        base.initOnEnable();

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

		//GetTransformArea();
    }

    public override void Attack()
    {
        base.Attack();
        transform.LookAt(_targetEnemyTransform);

        if (_targetEnemyTransform == null) return;
    }

    public override void Death()
    {
        base.Death();
        _allObjectTransforms.Remove(this.transform);
        //Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }
    public override void Move()
    {
        if (_pv.IsMine)
        {
            base.Move();

            Vector3 moveTarget = Vector3.zero;

            if (_targetEnemyTransform != null /*&& area == ObjectPosArea.Road*/)
            {
                moveTarget = _targetEnemyTransform.position;
            }
            else
            {
                if (line == ObjectLine.UpperLine)
                {
                    moveTarget = milestoneUpper[lineIdx].position;
                }
                else if (line == ObjectLine.LowerLine)
                {
                    moveTarget = milestoneLower[lineIdx].position;
                }

                if (gameObject.layer == LayerMask.NameToLayer("Human"))
                {
                    if (Vector3.Distance(transform.position, moveTarget) <= 0.3f || transform.position.x - moveTarget.x > 1.0f)
                        lineIdx++;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("Cyborg"))
                {
                    if (Vector3.Distance(transform.position, moveTarget) <= 0.3f || transform.position.x - moveTarget.x < 1.0f)
                        lineIdx--;
                }
            }

            transform.LookAt(new Vector3(
                moveTarget.x,
                transform.position.y,
                moveTarget.z
            ));
            nav.SetDestination(moveTarget);
        }
		else
		{
            Debug.Log("else moving");

            // 수신된 좌표로 보간한 이동처리
            transform.position = Vector3.Lerp(transform.position,
                                              receivePos,
                                              Time.deltaTime * damping);

            // 수신된 회전값으로 보간한 회전처리
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  receiveRot,
                                                  Time.deltaTime * damping);
        }
    }

    protected override void UpdateObjectAction()
    {
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
                break;
            case ObjectAction.Death:
                nav.enabled = false;
                transform.Find("UI").gameObject.SetActive(false);
                break;
            case ObjectAction.Move:
                nav.enabled = true;
                break;
            case ObjectAction.Idle:
                nav.enabled = false;
                break;
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

    private void GetTransformArea()
    {
        Vector3Int pos = grid.WorldToCell(this.transform.position);
        string posName = tilemap.GetTile(pos).name;

        area = ObjectPosArea.Undefine;

        if (posName.Equals("tilePalette_9"))  area = ObjectPosArea.Road;
        if (posName.Equals("tilePalette_1"))  area = ObjectPosArea.Building;
        if (posName.Equals("tilePalette_10")) area = ObjectPosArea.MidWay;
        if (posName.Equals("tilePalette_2"))  area = ObjectPosArea.CenterArea;
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