using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class objectController : MonoBehaviour
{
    private Tilemap tilemap;

    public string status;
    public string camp;
    
    public string nowArea;

    [Header ("- Stats Script")]
    [SerializeField] Stats stats;

    [Header ("- Children Script")]
    [SerializeField] objectMove moveScript;
    [SerializeField] objectAttack attackScript;
    [SerializeField] objectSummon summonScript;
    [SerializeField] objectDeath deathScript;

    [Header ("- Enemy Object in Recognition Range")]
    [SerializeField] Collider[] enemyListInRecognitionRange;

    void Start()
    {
        // Component 불러오기
        stats = GetComponent<Stats>();

        moveScript   = GetComponent<objectMove>();
        attackScript = GetComponent<objectAttack>();
        summonScript = GetComponent<objectSummon>();
        deathScript  = GetComponent<objectDeath>();

        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    void Update()
    {
        if (attackScript != null) 
        {
            GetNearbyEnemyObject();
        }
        
        if (moveScript != null)
        {
            SetArea();
        }
        
        SetStatus();
        PlayStatus();
    }

    private void SetArea()
    {
        nowArea = tilemap.GetTile(tilemap.WorldToCell(transform.position))?.name;
    }

    void SetStatus()
    {
        /*
            현재 상태를 변경해주는 함수
        */

        // 상태 변경
        if (deathScript != null && stats.NowHealth <= 0) 
        {   // 죽음 (현재 체력 체크)
            status = "Death";
        }
        else if (summonScript != null)
        {   // 소환
            status = "Summon";
        }
        else if (moveScript != null && (nowArea == "tilePalette_0" || enemyListInRecognitionRange.Length == 0)) 
        {   // 이동 (범위 내 적 확인)
            status = "Move";
        }
        else if (deathScript != null && enemyListInRecognitionRange.Length > 0)
        {   // 공격 
            status = "Attack";
        }
        else
        {   // 정지
            status = "None";
        }
    }

    void PlayStatus()
    {
        /*
            현재 상태에 따른 Ai 실행
        */

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

    void GetNearbyEnemyObject()
    {
        /*
            공격 범위 내 적들을 구하는 함수
        */

        enemyListInRecognitionRange = Physics.OverlapSphere(
            transform.position, // 현재 위치
            stats.RecognitionRange, // 인식 범위
            1 << (LayerMask.NameToLayer(camp == "Human" ? "Cyborg" : "Human")) // 적 진영
        );
    }
}
