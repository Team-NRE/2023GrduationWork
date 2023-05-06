using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BaseController
{
    public float planescale_X { get; set; }
    public float planescale_Z { get; set; }
    public float Cam_Y { get; set; }
    public float Cam_Z { get; set; }

    //속도
    private Vector3 velocity = Vector3.zero;

    //
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
        switch (_cameraMode)
        {
            case Define.CameraMode.QuaterView:
                QuaterviewCam();

                break;

            case Define.CameraMode.FloatCamera:
                FloatCam();

                break;
        }
    }

    public override void KeyDownAction(Define.KeyboardEvent _key)
    {
        if (_key == Define.KeyboardEvent.U)
        {
            _cameraMode = (_cameraMode == Define.CameraMode.FloatCamera ? Define.CameraMode.QuaterView : Define.CameraMode.FloatCamera);
        }
    }


    //카메라 이동
    public void FloatCam()
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
    public void QuaterviewCam()
    {
        //카메라 이동
        Vector3 pos = new Vector3(p_Position.position.x, p_Position.position.y + Cam_Y, p_Position.position.z - Cam_Z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);
    }


}
