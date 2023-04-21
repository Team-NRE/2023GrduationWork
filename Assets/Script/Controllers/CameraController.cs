using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BaseController
{
    public float _planescale_X;
    public float _planescale_Z;
    public float _Cam_Y;
    public float _Cam_Z;

    public float planescale_X { get { return _planescale_X; } set { _planescale_X = value; } }
    public float planescale_Z { get { return _planescale_Z; } set { _planescale_Z = value; } }
    public float Cam_Y { get { return _Cam_Y; } set { _Cam_Y = value; } }
    public float Cam_Z { get { return _Cam_Z; } set { _Cam_Z = value; } }

    //속도
    private Vector3 velocity = Vector3.zero;

    //카메라 고정 or 풀기
    private bool CameraFixed = false;
    private Transform p_Position;
    
    public override void Setting()
    {
        //초기 값 세팅
        planescale_X = 80; // -80 < X < 80
        planescale_Z = -4; // -28 < Z < 20 / +24
        Cam_Y = 9;
        Cam_Z = 6;

        p_Position = GameObject.Find("PlayerController").transform;
    }

    private void LateUpdate()
    {
        
        if (CameraFixed == true) { FixedCameraMove(); }
        else { CameraMove(); }
    }

    public override void KeyDownAction(string name)
    {
        if (name == "u") { CameraFixed = (CameraFixed == false ? true : false); }
    }


    //카메라 이동
    public void CameraMove()
    {
        //p_Position -> 플레이어의 이동해야할 위치

        //viewportPoint로 마우스 좌표 받기(x = 0~1/ y = 0~1)
        Vector3 MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //x 값
        Vector3 position_x_0 = new Vector3(-planescale_X, p_Position.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_1 = new Vector3(planescale_X, p_Position.position.y + Cam_Y, Camera.main.transform.position.z);

        //y 값
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, p_Position.position.y + Cam_Y, planescale_Z - 24);
        Vector3 position_z_1 = new Vector3(Camera.main.transform.position.x, p_Position.position.y + Cam_Y, planescale_Z + 24);

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
        Vector3 pos = new Vector3(p_Position.position.x, p_Position.position.y + Cam_Y, p_Position.position.z - Cam_Z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);
    }


}
