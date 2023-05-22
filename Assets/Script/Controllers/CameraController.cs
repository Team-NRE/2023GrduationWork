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

    private Transform p_Position;

    private GameObject player;
    public GameObject RendererGameobject;
    public List<GameObject> save;

    public bool IsRenderer = true;

    public override void Setting()
    {
        //초기 값 세팅
        planescale_X = 80; // -80 < X < 80
        planescale_Z = -4; // -28 < Z < 20 / +24
        Cam_Y = 9;
        Cam_Z = 6;

        player = GameObject.Find("PlayerController");
        p_Position = player.transform;
    }

    void Update()
    {
        //건물 투명화
        if (IsRenderer == false) hitRenderer();
        //건물 불투명화
        if (IsRenderer == true) re_Renderer();
    }

    public override void MouseDownAction(string _button)
    {
        if (_button == "rightButton") 
        {
            if(Get3DMousePosition().Item2.layer == 9) { Debug.Log("불투명화"); IsRenderer = true; }
            if(Get3DMousePosition().Item2.layer == 0) { Debug.Log("투명화"); IsRenderer = false; }
        }
    }

    private void re_Renderer()
    {
        for (int i = 0; i < save.Count; i++)
        {
            Material Mat = save[i].GetComponent<Renderer>().material;
            Mat.SetFloat("_Mode", 0);

            Mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            Mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            Mat.SetInt("_ZWrite", 1);
            Mat.DisableKeyword("_ALPHATEST_ON");
            Mat.DisableKeyword("_ALPHABLEND_ON");
            Mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            Mat.renderQueue = -1;

            //alpha값 조절
            Color matColor = Mat.color;
            matColor.a = 1f;
            Mat.color = matColor;

            save[i].layer = 0;
        }
        save.Clear();

    }
    private void hitRenderer()
    {
        float Distance = Vector3.Distance(transform.position, p_Position.position);

        //Player -> 카메라 방향 벡터의 정규화를 시켜 방향 정보 유지
        Vector3 Direction = (p_Position.position - transform.position).normalized;

        RaycastHit hit;

        int layerMask = ~(1 << 2);

        if (Physics.Raycast(transform.position, Direction, out hit, Distance, layerMask))
        {
            RendererGameobject = hit.collider.gameObject;
            // 2.맞았으면 Renderer를 얻어온다.
            Renderer ObstacleRenderer = RendererGameobject.GetComponent<Renderer>();

            //건물 속에 있을 때
            if (RendererGameobject.layer == (int)Define.Layer.Default)
            {
                // 3. Metrial의 Aplha를 바꾼다.
                Material Mat = ObstacleRenderer.material;
                // Rendering Mode를 변경하고 싶은 값으로 설정합니다.
                Mat.SetFloat("_Mode", 2); // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparent

                // 변경된 값을 적용합니다.
                Mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                Mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                Mat.SetInt("_ZWrite", 0);
                Mat.DisableKeyword("_ALPHATEST_ON");
                Mat.DisableKeyword("_ALPHABLEND_ON");
                Mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                Mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                //alpha값 조절
                Color matColor = Mat.color;
                matColor.a = 0.1f;
                Mat.color = matColor;

                if (save.IndexOf(RendererGameobject) == -1)
                {
                    RendererGameobject.layer = 2;
                    save.Add(RendererGameobject);
                }
            }
        }
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
