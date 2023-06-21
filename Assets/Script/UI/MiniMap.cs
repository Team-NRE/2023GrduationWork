/// ksPark
///
/// 미니맵 스크립트

using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public float planescale_X { get; set; }
    public float planescale_Z { get; set; }
    public float Cam_Y { get; set; }
    public float Cam_Z { get; set; }

    [SerializeField]
    Camera miniMapCamera;
    CameraController mainCamera;
    RawImage mapImage;
    RawImage cameraFrame;

    private void Awake()
    {
        //초기 값 세팅
        planescale_X = 80; // -80 < X < 80
        planescale_Z = -4; // -28 < Z < 20 / +24
        Cam_Y = 9;
        Cam_Z = 6;

        miniMapCamera = gameObject.GetComponentInChildren<Camera>();
        mainCamera = Camera.main.GetComponent<CameraController>();
        
        mapImage = transform.Find("Map").GetComponent<RawImage>();
        cameraFrame = transform.Find("Frame").GetComponent<RawImage>();
    }

    private void Update()
    {
        setFramePositionToCameraPosition();
    }

    private void setFramePositionToCameraPosition()
    {

    }
}
