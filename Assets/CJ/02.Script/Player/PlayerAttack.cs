using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    //타겟
    public GameObject target;
    float Best_T = 999;

    //vector
    public Vector3 Point; // 마우스 클릭시 포인터 위치

    //bool  
    bool checkAttack = true; //사거리 표시 On/Off 유무
    public bool attack = false; //어택시 on/off 유무

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
        projector.orthographicSize = PlayerManager.Player_Instance.player_stats.attackRange;
        GetAttackRange();
    }


    void Update()
    {
        //어택 체크
        StartCoroutine(CheckAttackRoutine());
    }


    //A 온 오프 코드
    public void Attack()
    {
        //a키 누르기 -> 사거리 켜기 -> 원 둘레 안 적 식별 -> 적이면 애니메이션 및 공격 작용.
        GetAttackRange();
    }


    //사거리 표시 껏다 켜기
    public GameObject GetAttackRange()
    {
        target = null;

        //이미지 체크
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

        //사거리 이미지가 on 이면
        if (checkAttack == true)
        {
            //플레이어 근처 적 식별
            Collider[] colls = Physics.OverlapSphere(transform.position, PlayerManager.Player_Instance.player_stats.attackRange, layerMask);


            Debug.Log("check");

            //마우스 왼쪽 클릭시
            if (Input.GetMouseButtonDown(0))
            {
                //식별된 적 모두 중 하나만 선별
                for (int i = 0; i < colls.Length; ++i)
                {
                    // ray로 마우스 위치 world 좌표로 받기.
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 100f);

                    if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
                    {
                        //마우스 포인터 위치
                        Point = raycastHit.point;

                        Debug.Log(Point);

                        Best_Target_Search(colls[i], Point);
                    }
                }

                Best_Target_Rotate(target);
                Shoot(target);
            }
        }
    }


    public GameObject Best_Target_Search(Collider colls, Vector3 point)
    {

        float Now_X = (point.x >= colls.transform.position.x) ? (Now_X = point.x - colls.transform.position.x)
                        : (Now_X = colls.transform.position.x - point.x);

        float Now_Z = (point.z >= colls.transform.position.z) ? (Now_Z = point.z - colls.transform.position.z)
                        : (Now_Z = colls.transform.position.x - Point.z);

        if (Best_T >= Now_X + Now_Z)
        {
            Best_T = Now_X + Now_Z;
            target = colls.gameObject;
        }
        

        return target;
    }

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
        attack = true;


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
        if (bulletPrefab != null)
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
            bullet.GetComponent<PlayerBullet>().target_set(target, shotPower);
        }

        GetAttackRange(); //사거리 이미지 off
    }

}
