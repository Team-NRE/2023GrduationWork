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
    public Vector3 velocity = Vector3.zero;

    //카메라 고정 or 풀기
    bool CameraFixed = false;

    public GameObject player;


    public override void Setting()
    {
        //초기 값 세팅
        planescale_X = 17;
        planescale_Z = 9;
        Cam_Y = 9;
        Cam_Z = 6;

        player = GameObject.FindWithTag("PLAYER");
    }

    public override void KeyDownAction(string name)
    {
        CameraChange();
    }

    private void LateUpdate()
    {
        CameraChange();
        KeyDownAction("u");
    }

    private void CameraChange()
    {
        CameraFixed = (CameraFixed == false ? true : false);

        if (CameraFixed == true) { FixedCameraMove(); }
        else { CameraMove(); }
    }


    //카메라 이동
    public void CameraMove()
    {
        //Plane 스케일 
        float Scale_X = 5 * planescale_X + 5;
        float Scale_Z = 5 * planescale_Z + 5;

        //viewportPoint로 마우스 좌표 받기(x = 0~1/ y = 0~1)
        Vector3 MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //x 값
        Vector3 position_x_0 = new Vector3(-Scale_X, player.transform.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_1 = new Vector3(Scale_X, player.transform.position.y + Cam_Y, Camera.main.transform.position.z);

        //y 값
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, player.transform.position.y + Cam_Y, -Scale_Z);
        Vector3 position_z_1 = new Vector3(Camera.main.transform.position.x, player.transform.position.y + Cam_Y, Scale_Z);

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
