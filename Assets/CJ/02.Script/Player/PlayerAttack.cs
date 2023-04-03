using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //bool  
    bool checkAttack = true;
    
    //레이어
    public LayerMask layerMask;

    //사거리 표시
    public GameObject AttackRangeimg;

    //총알
    public GameObject bulletPrefab;
    //총알 Flash
    public GameObject muzzleFlashPrefab;
    //총알 위치
    [SerializeField] private Transform barrelLocation;

    //총알 파괴
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    //총알 속도
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;

    private void Awake()
    {
        //공격사거리 세팅
        Projector projector = AttackRangeimg.GetComponent<Projector>();
        projector.orthographicSize = PlayerManager.Player_Instance.player_stats.attackRange;
        GetAttackRange();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           Attack();
        }

        //어택 체크
        StartCoroutine(CheckAttackRoutine());
    }

    //Attack
    void Attack()
    {
        //a키 누르기 -> 사거리 켜기 -> 원 둘레 안 적 식별 -> 적이면 애니메이션 및 공격 작용.
        GetAttackRange();
        
    }

    //사거리 표시 껏다 켜기
    public GameObject GetAttackRange()
    {
        if (checkAttack == false || checkAttack == true)
        {
            checkAttack = !checkAttack;
            AttackRangeimg.SetActive(checkAttack);
        }

        return AttackRangeimg;
    }
    
    //어택 범위 설정
    IEnumerator CheckAttackRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (checkAttack == true)
        {
            Attack_Detection(transform.position, PlayerManager.Player_Instance.player_stats.attackRange);
        }
    }


    //상대방한테 Check.cs 달아줘야 공격 인식 -> 개선필요.
    void Attack_Detection(Vector3 pos, float radius)
    {
        Collider[] colls = Physics.OverlapSphere(pos, radius, layerMask);

        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].SendMessage("Check", SendMessageOptions.DontRequireReceiver);
        }

    }

    //총알 발사 -> 개선 필요
    public void Shoot()
    {
        //state = State.Attack;
        
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bulletPrefab.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        GetAttackRange();
    }

}
