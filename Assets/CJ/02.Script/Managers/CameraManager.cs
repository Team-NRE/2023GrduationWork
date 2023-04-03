using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("---PlaneScale---")]
    public float planescale;

    [Header("---Camera---")]
    //카메라 z축
    [Range(2.0f, 100.0f)]
    public float Cam_Z;
    
    //카메라 y축
    [Range(0.0f, 100.0f)]
    public float Cam_Y;
    public Vector3 MousePos;

    //속도
    public Vector3 velocity = Vector3.zero;
    
    public GameObject player;
    
    public void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
    }


    public void FixedUpdate()
    {
        CameraMove(); //카메라 움직임
    } 


    //카메라 이동
    public void CameraMove()
    {
        //Plane 스케일 
        float Scale = 5 * planescale + 5;
        //viewportPoint로 마우스 좌표 받기(x = 0~1/ y = 0~1)
        MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //x 값
        Vector3 position_x_0 = new Vector3(-Scale, player.transform.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_1 = new Vector3(Scale, player.transform.position.y + Cam_Y, Camera.main.transform.position.z);

        //y 값
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, player.transform.position.y + Cam_Y, -Scale);
        Vector3 position_z_1 = new Vector3(Camera.main.transform.position.x, player.transform.position.y + Cam_Y, Scale);

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
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + Cam_Y, player.transform.position.z - Cam_Z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);    
    }


}
