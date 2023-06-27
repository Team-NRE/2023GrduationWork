/// ksPark
/// 
/// 넥서스의 상세 코드 스크립트

using UnityEngine;
using Define;
using Photon.Pun;

public class Nexus : ObjectController
{

    public override void init() 
    {
        base.init();
       // _pv = GetComponent<PhotonView>();
        _type = ObjectType.Nexus;
    }

    public override void Death()
    {
        base.Death();
        //gameObject.SetActive(false);
        gameFinish();
    }

    protected override void UpdateObjectAction()
    {
        if (_oStats.nowHealth <= 0) _action = ObjectAction.Death;
        else _action = ObjectAction.Idle;
    }

    public void disablePlay()
    {

    }

    public void gameFinish()
    {

    }
}