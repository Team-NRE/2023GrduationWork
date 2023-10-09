/// ksPark
///
/// 미니맵 스크립트

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

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
            SetFramePositionToCameraPosition();
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

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MoveCharacter();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isButtonDowning = false;
        }
    }

    private void MoveCharacter()
    {
        Players p = Managers.game.myCharacter?.GetComponent<Players>();
        Vector3 newPos = MapFieldPositionConverter();
        NavMeshHit hit;

        if (p == null) return;
        if (!NavMesh.SamplePosition(newPos, out hit, 2.0f, NavMesh.AllAreas)) return;
        
        p.RightButtonTargetSetting(hit.position);
    }

    private void SetFramePositionToCameraPosition()
    {
        Vector3 newPos = MapFieldPositionConverter();
        newPos.y = mainCamera.transform.position.y;
        newPos.z += mainCamera.Cam_Z;
        mainCamera.transform.position = newPos;
    }

    private Vector3 MapFieldPositionConverter()
    {
        Vector2 pos;
        Vector3 mousePos = Input.mousePosition, newPos = Vector3.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTransform, mousePos, canvas.worldCamera, out pos);

        newPos.x = Mathf.Lerp(mainCamera.mapMin.x, mainCamera.mapMax.x, Normalizing(-mapRectTransform.rect.width/2, mapRectTransform.rect.width/2, pos.x));
        newPos.z = Mathf.Lerp(mainCamera.mapMin.z, mainCamera.mapMax.z, Normalizing(-mapRectTransform.rect.height/2, mapRectTransform.rect.height/2, pos.y));

        return newPos;
    }

    private float Normalizing(float min, float max, float value)
    {
        return (value - min) / (max - min);
    }
}
