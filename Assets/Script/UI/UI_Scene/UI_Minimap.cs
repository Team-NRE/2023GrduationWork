/// ksPark
///
/// 미니맵 스크립트

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Minimap : UI_Scene, IPointerDownHandler, IPointerUpHandler
{
    CameraController mainCamera;
    Canvas canvas;
    RectTransform mapRectTransform;

    bool isButtonDowning { get; set; }

    private void Awake()
    {
        //초기 값 세팅
        mainCamera  = Camera.main.GetComponent<CameraController>();
        canvas      = GetComponent<Canvas>();
        mapRectTransform = transform.Find("Map").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Managers.game.isGameEnd) return;
        
        if(isButtonDowning)
        {
            mainCamera.enabled = false;
            setFramePositionToCameraPosition();
        }
        else
        {
            mainCamera.enabled = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isButtonDowning = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isButtonDowning = false;
        }
    }

    private void setFramePositionToCameraPosition()
    {
        Vector2 pos;
        Vector3 mousePos = Input.mousePosition, newPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTransform, mousePos, canvas.worldCamera, out pos);

        newPos.x = Mathf.Lerp(mainCamera.mapMin.x, mainCamera.mapMax.x, normalizing(-mapRectTransform.rect.width/2, mapRectTransform.rect.width/2, pos.x));
        newPos.y = mainCamera.transform.position.y;
        newPos.z = Mathf.Lerp(mainCamera.mapMin.z, mainCamera.mapMax.z, normalizing(-mapRectTransform.rect.height/2, mapRectTransform.rect.height/2, pos.y)) + mainCamera.Cam_Z;

        mainCamera.transform.position = newPos;
    }

    private float normalizing(float min, float max, float value)
    {
        return (value - min) / (max - min);
    }
}
