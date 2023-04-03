using UnityEngine;
using UnityEngine.AI;

public partial class PlayerManager 
{
    [Header("---Player Move---")]
    //NavMeshAgent
    public NavMeshAgent agent;
    //Transform
    public new Transform transform;
    //이동할 점
    public Vector3 Point;
    //속도
    public Vector3 velocity = Vector3.zero;
    //남은거리
    public float remainDistance;

    //카메라 이동
    public void CameraMove()
    {
        //Plane 스케일 
        float Scale = 5 * planescale + 5;
        //viewportPoint로 마우스 좌표 받기(x = 0~1/ y = 0~1)
        MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //x 값
        Vector3 position_x_0 = new Vector3(-Scale, transform.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_1 = new Vector3(Scale, transform.position.y + Cam_Y, Camera.main.transform.position.z);

        //y 값
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, transform.position.y + Cam_Y, -Scale);
        Vector3 position_z_1 = new Vector3(Camera.main.transform.position.x, transform.position.y + Cam_Y, Scale);

        if (MousePos.x <= 0)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x_0, Time.deltaTime);
        }

        if (MousePos.x >= 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x_1, Time.deltaTime);
        }

        if (MousePos.y <= 0)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z_0, Time.deltaTime);
        }

        if (MousePos.y >= 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z_1, Time.deltaTime);
        }
    }

    //고정 카메라 이동
    public void FixedCameraMove()
    {
        //카메라 이동
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + Cam_Y, transform.position.z - Cam_Z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);    
    }

    //플레이어 이동
    public void PlayerMove()
    {
        // ray로 마우스 위치 world 좌표로 받기.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //광선 그려주기
        //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 1f);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ~(Ignorelayer)))
        {
            Point = raycastHit.point;

            //각도
            Point.y = 0f;
            float dx = Point.x - transform.position.x;
            float dz = Point.z - transform.position.z;
            float rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dz, dx) - 90); //tan-1(dz/dx) = 각도
            //레어와 닿은 곳으로 회전
            transform.eulerAngles = new Vector3(0f, rotDegree, 0f);

            agent.SetDestination(Point);
            
        }
    }

}
