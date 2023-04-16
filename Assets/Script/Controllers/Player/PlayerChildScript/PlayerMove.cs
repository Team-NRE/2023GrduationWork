using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [Header("---Player Move---")]
    //이동할 점
    public Vector3 Point;
    //속도
    public Vector3 velocity = Vector3.zero;

    [Header("---Move Ignore Layer---")]
    public LayerMask Ignorelayer;

    NavMeshAgent agent;
    
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
      
    //플레이어 이동 
    public void playerMove()
    {
        // ray로 마우스 위치 world 좌표로 받기.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //광선 그려주기
        //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 100f);
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