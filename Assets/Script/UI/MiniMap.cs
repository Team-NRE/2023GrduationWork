/// ksPark
///
/// 미니맵 스크립트

using UnityEngine;
using UnityEngine.UI;
using Define;

public class MiniMap : MonoBehaviour
{
    public float planescale_X { get; set; }
    public float planescale_Z { get; set; }
    public float Cam_Y { get; set; }
    public float Cam_Z { get; set; }

    [SerializeField]
    CameraController mainCamera;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    RectTransform mapRectTransform;

    public bool m_IsButtonDowning;

    private void Awake()
    {
        //초기 값 세팅
        planescale_X = 256f; // -80 < X < 80
        planescale_Z = 89.6f; // -28 < Z < 20 / +24
        Cam_Y = 9;
        Cam_Z = 6;

        mainCamera = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        if(m_IsButtonDowning)
        {
            setFramePositionToCameraPosition();
        }
    }

    public void PointerDown()
    {
        m_IsButtonDowning = true;
    }

    public void PointerUp()
    {
        m_IsButtonDowning = false;
    }

    private void setFramePositionToCameraPosition()
    {
        Vector2 pos;
        Vector3 mousePos = Input.mousePosition, newPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTransform, mousePos, canvas.worldCamera, out pos);

        newPos.x = Mathf.Lerp(-mainCamera.planescale_X, mainCamera.planescale_X, normalizing(-planescale_X, planescale_X, pos.x));
        newPos.y = mainCamera.transform.position.y;
        newPos.z = Mathf.Lerp(mainCamera.planescale_Z - 26, mainCamera.planescale_Z + 24, normalizing(-planescale_Z, planescale_Z, pos.y));

        mainCamera.transform.position = newPos;
    }

    private float normalizing(float min, float max, float value)
    {
        return (value - min) / (max - min);
    }
}
