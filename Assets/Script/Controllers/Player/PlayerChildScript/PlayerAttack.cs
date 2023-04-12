using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    //vector
    public Vector3 Point; // 마우스 클릭시 포인터 위치

    //bool  
    public bool checkAttack = true; //사거리 표시 On/Off 유무
    public bool attack = false; //attack 애니메이션

    //레이어
    public LayerMask layerMask;

    //사거리 표시 object
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
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 100f;


    private void Start()
    {
        
        //공격사거리 세팅
        Projector projector = AttackRangeimg.GetComponent<Projector>();
        projector.orthographicSize = PlayerController.Player_Instance.player_stats._attackRange;
        GetAttackRange();
    }


    void Update()
    {
        //어택 체크
        if (checkAttack == true && Input.GetMouseButtonDown(0))
        {
            CheckAttackRoutine();
        }
    }

    //사거리 표시 껏다 켜기
    public GameObject GetAttackRange()
    {
        //이미지 체크
        if (checkAttack == false || checkAttack == true)
        {
            checkAttack = !checkAttack;
            //사거리 이미지 On/off
            AttackRangeimg.SetActive(checkAttack);
        }

        return AttackRangeimg;
    }


    //어택 범위 설정
    public void CheckAttackRoutine()
    {
        //타겟 초기화
        GameObject target = null;
        //가장 가까운 거리 초기화
        float CloseDistance = Mathf.Infinity;
        // ray로 마우스 위치 world 좌표로 받기.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            //마우스 포인터 위치
            Point = raycastHit.point;
        }

        //플레이어 근처 적 식별
        Collider[] colls = Physics.OverlapSphere(transform.position, PlayerController.Player_Instance.player_stats._attackRange, layerMask);
        //식별된 적 모두 중 하나만 선별
        foreach(Collider coll in colls)
        {
            //마우스 포인터, 식별된 적 사이의 거리 구하기
            float distance = Vector3.Distance(Point, coll.transform.position);
            //가장 가까운 거리 설정 및 타겟 설정
            if(CloseDistance > distance)
            {
                CloseDistance = distance;
                target = coll.gameObject;
            }
        }
        
        //타겟이 설정된다면
        if(target != null)
        {
            Best_Target_Rotate(target);
            Shoot(target);
        }
    }

    //타겟을 향해 몸 돌리기
    public void Best_Target_Rotate(GameObject target)
    {
        //공격시 적을 바라봐야하는 코드(플레이어의 Rotation)
        Vector3 target_pos = target.transform.position;
        target_pos.y = 0f;
        float dx = target_pos.x - transform.position.x;
        float dz = target_pos.z - transform.position.z;
        float rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dz, dx) - 90); //tan-1(dz/dx) = 각도
        transform.rotation = Quaternion.Euler(0f, rotDegree, 0f);
    }

    //총알 발사
    public void Shoot(GameObject target)
    {
        if (bulletPrefab == null) { Debug.Log("ㄴ"); }

        //어택 애니메이션 작동을 위해 bool 값
        PlayerController.Player_Instance.player_key._key = "Attack";


        //총알 발사 이미지
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }


        //총알 프리팹
        if (bulletPrefab != null && target != null)
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
            bullet.GetComponent<PlayerBullet>().target_set(target, shotPower);
        }

        GetAttackRange(); //사거리 이미지 off
    }

}
