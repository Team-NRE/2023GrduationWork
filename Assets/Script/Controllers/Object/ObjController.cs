using UnityEngine;
using UnityEngine.Tilemaps;
using Stat;

public abstract class ObjController : MonoBehaviour
{
    /// <summary>타일 맵 오브젝트</summary>
    private Tilemap tilemap;

    /// <summary>오브젝트 상태</summary>
    public string status;
    /// <summary>진영 정보</summary>
    public string camp;
    
    /// <summary>타일 지역 상태</summary>
    public string nowArea;

    /// <summary>스텟 스크립트</summary>
    [Header ("- Stats Script")]
    [SerializeField] ObjStats stats;

    /// <summary>오브젝트 이동 스크립트</summary>
    [Header ("- Children Script")]
    [SerializeField] MinionMove moveScript;
    /// <summary>오브젝트 공격 스크립트</summary>
    [SerializeField] ObjAttack attackScript;
    /// <summary>오브젝트 소환 스크립트</summary>
    [SerializeField] ObjSummon summonScript;
    /// <summary>오브젝트 사망 스크립트</summary>
    [SerializeField] ObjDeath deathScript;

    /// <summary>인식 범위 내 적 목록</summary>
    [Header ("- Enemy Object in Recognition Range")]
    [SerializeField] Collider[] enemyListInRecognitionRange;

    void Start()
    {
        // Component 불러오기
        stats = GetComponent<ObjStats>();

        moveScript   = GetComponent<MinionMove>();
        attackScript = GetComponent<ObjAttack>();
        summonScript = GetComponent<ObjSummon>();
        deathScript  = GetComponent<ObjDeath>();

        tilemap = GameObject.Find("Tilemap")?.GetComponent<Tilemap>();
    }

    void Update()
    {
        if (!Object.ReferenceEquals(attackScript, null))
        {
            GetNearbyEnemyObject();
        }

        if (!Object.ReferenceEquals(moveScript, null))
        {
            SetArea();
        }
        
        SetStatus();
        PlayStatus();
    }

    /// <summary>
    /// 현재 서 있는 타일 지역을 가져오는 함수
    /// </summary>
    private void SetArea()
    {
        nowArea = tilemap.GetTile(tilemap.WorldToCell(transform.position))?.name;
    }

    /// <summary>
    /// 현재 상태를 변경해주는 함수
    /// </summary>
    void SetStatus()
    {
        // 상태 변경
        if (!Object.ReferenceEquals(deathScript, null) && stats.NowHealth <= 0) 
        {   // 죽음 (현재 체력 체크)
            status = "Death";
        }
        else if (!Object.ReferenceEquals(summonScript, null))
        {   // 소환
            status = "Summon";
        }
        else if (!Object.ReferenceEquals(moveScript, null) && (nowArea == "tilePalette_0" || enemyListInRecognitionRange.Length == 0)) 
        {   // 이동 (범위 내 적 확인)
            status = "Move";
        }
        else if (!Object.ReferenceEquals(attackScript, null) && enemyListInRecognitionRange.Length > 0)
        {   // 공격 
            status = "Attack";
        }
        else
        {   // 정지
            status = "None";
        }
    }

    /// <summary>
    /// 현재 상태에 따른 A 실행
    /// </summary>
    void PlayStatus()
    {
        switch (status)
        {
            case "Death":
                deathScript.Death();
                break;
            case "Summon":
                summonScript.Summon(camp);
                break;
            case "Attack":
                attackScript.Attack(nowArea, enemyListInRecognitionRange);
                break;
            case "Move":
                moveScript.Move(nowArea, camp);
                break;
        }
    }

    /// <summary>
    /// 공격 범위 내 적들을 구하는 함수
    /// </summary>
    void GetNearbyEnemyObject()
    {
        enemyListInRecognitionRange = Physics.OverlapSphere(
            transform.position, // 현재 위치
            stats.RecognitionRange, // 인식 범위
            1 << (LayerMask.NameToLayer(camp == "Human" ? "Cyborg" : "Human")) // 적 진영
        );
    }
}
