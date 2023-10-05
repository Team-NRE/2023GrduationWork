using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BaseController
{
    // 맵 실제 크기
    public Vector3 mapMax;
    public Vector3 mapMin;
    public float Cam_Y { get; set; }
    public float Cam_Z { get; set; }
    Vector3 cameraFieldMax;
    Vector3 cameraFieldMin;

    // 화면 비율
    float screenRatio = 1920 / 1080;

    //속도
    private Vector3 velocity = Vector3.zero;

    //Player 찾기
    private Transform p_Position;

    //건물 투명화
    public GameObject RendererGameobject;
    public List<GameObject> SaveRendererModel;

    //Renderer 여부
    public bool IsRenderer = true;

    //투명화 되는 건물 색깔
    public Color invisibleColor;

    LayerMask ignore;

    public override void Init()
    {
        p_Position = Managers.game.myCharacter?.transform;

        //초기 값 세팅
        Cam_Y = 9;
        Cam_Z = -6;

        mapMax = GameObject.Find("MapMax").transform.position;
        mapMin = GameObject.Find("MapMin").transform.position;

        InitCameraField();

        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;

        ignore = LayerMask.GetMask("Human", "Cyborg");
        Managers.UI.isOpenedPopup = false;
    }

    void InitCameraField()
    {
        cameraFieldMax = mapMax;
        cameraFieldMin = mapMin;

        cameraFieldMax.x += Cam_Z * screenRatio;
        cameraFieldMin.x -= Cam_Z * screenRatio;
        cameraFieldMax.z += Cam_Z * 2;
    }

    private void OnDisable()
    {
        _cameraMode = Define.CameraMode.FloatCamera;
    }

    public override void Update()
    {
        if (p_Position == null)
        {
            p_Position = Managers.game.myCharacter?.transform;

            if (p_Position != null)
            {
                Camera.main.transform.position = new Vector3(
                    p_Position.position.x, 
                    p_Position.position.y + Cam_Y, 
                    p_Position.position.z + Cam_Z
                );
            }

            return;
        }

        //건물 불투명화
        if (IsRenderer == true) re_Renderer();
        //건물 투명화
        if (IsRenderer == false) hitRenderer();
    }


    private void LateUpdate()
    {
        if (p_Position == null)
        {
            p_Position = Managers.game.myCharacter?.transform;

            if (p_Position != null)
            {
                Camera.main.transform.position = new Vector3(
                    p_Position.position.x, 
                    p_Position.position.y + Cam_Y, 
                    p_Position.position.z + Cam_Z
                );
            }
            
            return;
        }
        
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

    //마우스 이벤트
    public void MouseDownAction(Define.MouseEvent _evt)
    {
        if (_evt == Define.MouseEvent.PointerDown || _evt == Define.MouseEvent.Press)
        {
            GameObject hitObject = Managers.Input.Get3DMousePosition(ignore).Item2;

            if (hitObject != null)
            {
                switch (hitObject.layer)
                {
                    case (int)Define.Layer.Road:
                        IsRenderer = true;
                        break;

                    case (int)Define.Layer.Default:
                        IsRenderer = false;
                        break;
                }
            }
        }
    }

    //키보드 이벤트
    public void KeyDownAction(Define.KeyboardEvent _key)
    {
        if (_key == Define.KeyboardEvent.U)
        {
            _cameraMode = (_cameraMode == Define.CameraMode.FloatCamera ? Define.CameraMode.QuaterView : Define.CameraMode.FloatCamera);
        }

        //이동 카메라 일 때 Spacebar 꾹 누를 시 
        if (_key == Define.KeyboardEvent.Space)
        {
            //카메라 고정
            _cameraMode = Define.CameraMode.QuaterView;
        }

        //Space 키 땠을 때 
        if (_key == Define.KeyboardEvent.SpaceUp)
        {
            _cameraMode = Define.CameraMode.FloatCamera;
        }
    }


    //불투명화
    private void re_Renderer()
    {
        for (int i = 0; i < SaveRendererModel.Count; i++)
        {
            if (SaveRendererModel[i].tag == "OBJECT") continue;
            if (SaveRendererModel[i].tag == "PLAYER") continue;

            Material Mat = SaveRendererModel[i].GetComponent<Renderer>().material;
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
            matColor = Color.white;
            matColor.a = 1f;
            Mat.color = matColor;

            SaveRendererModel[i].layer = 0;

        }
        SaveRendererModel.Clear();

    }


    //투명화
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
                if (RendererGameobject.tag == "OBJECT") return;
                if (RendererGameobject.tag == "PLAYER") return;

                // 3. Metrial의 Aplha를 바꾼다.
                Material Mat = ObstacleRenderer.material;
                // Rendering Mode를 변경하고 싶은 값으로 설정합니다.
                Mat.SetFloat("_Mode", 2); // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparents

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
                matColor = invisibleColor;
                matColor.a = 0.3f;
                Mat.color = matColor;

                if (SaveRendererModel.IndexOf(RendererGameobject) == -1)
                {
                    RendererGameobject.layer = 2;
                    SaveRendererModel.Add(RendererGameobject);
                }
            }
        }
    }


    //카메라 이동
    public void FloatCam()
    {
        if (Managers.UI.isOpenedPopup) return;
        //p_Position -> 플레이어의 이동해야할 위치

        //viewportPoint로 마우스 좌표 받기(x = 0~1/ y = 0~1)
        Vector3 MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //x 값
        Vector3 position_x_0 = new Vector3(cameraFieldMin.x, p_Position.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_1 = new Vector3(cameraFieldMax.x, p_Position.position.y + Cam_Y, Camera.main.transform.position.z);

        //y 값
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, p_Position.position.y + Cam_Y, cameraFieldMin.z);
        Vector3 position_z_1 = new Vector3(Camera.main.transform.position.x, p_Position.position.y + Cam_Y, cameraFieldMax.z);

        //마우스가 창 밖으로 갈 때
        if (MousePos.x <= 0.1)
        {
           Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x_0, Time.deltaTime);
        }

        if (MousePos.x >= 0.9)
        {
           Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x_1, Time.deltaTime);
        }

        if (MousePos.y <= 0.1)
        {
           Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z_0, Time.deltaTime);
        }

        if (MousePos.y >= 0.9)
        {
           Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z_1, Time.deltaTime);
        }
    }


    //카메라 고정
    public void QuaterviewCam()
    {
        //카메라 이동
        Vector3 pos = new Vector3(p_Position.position.x, p_Position.position.y + Cam_Y, p_Position.position.z + Cam_Z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);
    }
}