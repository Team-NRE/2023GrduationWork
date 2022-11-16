using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovePhotonAdd : MonoBehaviourPun, IPunObservable
{
    float speed = 8.0f;
    public Vector3 movePoint;

    new Rigidbody rigidbody;

    //camera (z,y,속도)
    Camera viewCamera;
    [Range(2.0f, 100.0f)]
    public float Cam_Z = 10.0f;

    [Range(0.0f, 100.0f)]
    public float Cam_Y = 15.0f;
    Vector3 velocity = Vector3.zero;

    public LayerMask Ignorelayer;

    //이동할 위치값
    Vector3 thisUpdatePoint;

    //업데이트 될 플레이어 각도
    float rotDegree;

    //플레이어 네트워크 ID
    private PhotonView pv;

    //네트워크 동기화에 필요한 플레이어 위치 정보
    private Transform tr;
    private Vector3 receivePos;
    private Quaternion receiveRot;
    
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        pv = GetComponent<PhotonView>();
        tr = GetComponent<Transform>();
    }

    
    void Update()
    {
        if (pv.IsMine)
        {
            //오른쪽 버튼 누를 시
            if (Input.GetMouseButtonDown(1))
            {
                // ray로 마우스 위치 world 좌표로 받기.
                Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

                //광선 그려주기
                Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 1f);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ~(Ignorelayer)))
                {
                    //레어와 닿은 곳이 이동 포인트
                    movePoint = raycastHit.point;

                    //각도
                    movePoint.y = 0f;
                    float dx = movePoint.x - rigidbody.position.x;
                    float dz = movePoint.z - rigidbody.position.z;
                    rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dz, dx) - 90); //tan-1(dz/dx) = 각도

                    //Debug.Log("movepoint : " + movePoint.ToString());
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(rigidbody.position, movePoint) >= 0.1f)
        {
            // thisUpdatePoint 는 이번 업데이트(프레임) 에서 이동할 포인트를 담는 변수다.
            // 이동할 방향(이동할 곳-현재 위치) 곱하기 속도를 해서 이동할 위치값을 계산한다.
            thisUpdatePoint = (movePoint - rigidbody.position).normalized * speed;

            //플레이어 이동 및 회전
            rigidbody.MovePosition(rigidbody.position + thisUpdatePoint * Time.fixedDeltaTime);
            rigidbody.MoveRotation(Quaternion.Euler(0f, rotDegree, 0f));

            //Debug.Log(rotDegree.ToString());
        }
    }

    void LateUpdate() {
        //카메라 이동
        if (pv.IsMine)
        {
            Vector3 pos = new Vector3(rigidbody.position.x, rigidbody.position.y + Cam_Y, rigidbody.position.z - Cam_Z);
            viewCamera.transform.position = Vector3.SmoothDamp(viewCamera.transform.position, pos, ref velocity, 0.25f);
        }
    }

    //이동 transform 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}




