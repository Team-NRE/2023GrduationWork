/// ksPark
/// 
/// 넥서스의 상세 코드 스크립트

using UnityEngine;
using Define;

public class Nexus : ObjectController
{
    CameraController mainCamera;
    public Vector3 camPos;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Nexus;
        mainCamera = Camera.main.GetComponent<CameraController>();
    }

    public override void Death()
    {
        base.Death();
        transform.Find("UI").gameObject.SetActive(false);
        gameFinish();
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0) _action = ObjectAction.Death;
        else _action = ObjectAction.Idle;
    }

    public void disablePlay()
    {
        GameManager.Instance.setGameEnd(this.transform.position + Vector3.up * mainCamera.Cam_Y + Vector3.back * mainCamera.Cam_Z);
    }

    public void gameFinish()
    {
        
    }
}