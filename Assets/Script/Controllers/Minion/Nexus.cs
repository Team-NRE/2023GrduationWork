/// ksPark
/// 
/// 넥서스의 상세 코드 스크립트

using UnityEngine;
using Define;

public class Nexus : ObjectController
{
    [SerializeField]
    CameraController mainCamera;
    public Vector3 camPos;
    bool isInitWhenPlayer = false;

    public override void init() 
    {
        base.init();
        _type = ObjectType.Nexus;
        mainCamera = Camera.main.GetComponent<CameraController>();
    }

    public override void Death()
    {
        base.Death();
        gameFinish();
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0) _action = ObjectAction.Death;
        else _action = ObjectAction.Idle;

        if (!isInitWhenPlayer && Managers.game.myCharacter != null)
        {
            animator.SetBool("isVictory", Managers.game.myCharacter.layer != gameObject.layer);
            isInitWhenPlayer = true;
        }
    }

    public void disablePlay()
    {
        Vector3 endingCamPos = this.transform.position;
        endingCamPos += 2 * Vector3.up * mainCamera.Cam_Y;
        endingCamPos += 2 * Vector3.back * mainCamera.Cam_Z;

        transform.Find("UI").gameObject.SetActive(false);

        Managers.game.setGameEnd(endingCamPos);
    }

    public void gameFinish()
    {
        
    }
}