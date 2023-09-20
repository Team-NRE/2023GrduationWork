using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;
using Photon.Pun;

public class UI_Card : UI_Scene
{
    //카드 비용
    public int _cost;
    public int _cardBuyCost;

    //카드 시전 시간
    public float _CastingTime;

    //이펙트 발동 시간
    public float _effectTime;

    
    //Stat 
    public float _damage;
    public float _defence;
    public float _speed;
    public float _buff;
    public float _debuff;
    //부활
    public bool _IsResurrection;


    //스킬 범위 타입
    //Arrow = _rangeScale 고정 / Cone = _rangeScale, _rangeAngle / Line = _rangeScale 
    //Point = _rangeScale, _rangeRange / Range = _rangeScale
    public CardType _rangeType;
    //스킬 범위 크기
    public float _rangeScale;
    //스킬 거리
    public float _rangeRange;
    //스킬 각도
    public float _rangeAngle;

    public GameObject _effectObject;
    
    public override void Init()
    {
        Debug.Log("UI_Card Init");
    }


    public virtual void InitCard()
    {
        //하위 카드 컴포넌트에서 구현하여 사용 위함
    }



    
    public virtual void OnTriggerEnter(Collider other) 
    {
        
    }


    public virtual GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        return _effectObject;
    }

    public virtual void DestroyCard(float delay = default)
    {
        //하위 카드 컴포넌트에서 구현하여 사용 위함
    }

    public virtual GameObject RemoteTargetFinder(int id)
	{
        GameObject remoteTarget = PhotonView.Find(id).gameObject;
        return remoteTarget;
	}

    [PunRPC]
    protected GameObject RemoteAddComponent(int remoteId)
    {
        GameObject particle = Managers.game.RemoteTargetFinder(remoteId);
        particle.AddComponent<BaseEffect>();
        return particle;
    }

    [PunRPC]
    protected void GetRemoteParent(int objectId, int particleId)
    {
        GameObject playerObject = RemoteTargetFinder(objectId);
        GameObject particleObject = RemoteTargetFinder(particleId);

        particleObject.transform.SetParent(playerObject.transform);
    }

    [PunRPC]
    protected void RemoteLogger(string log)
    {
        Debug.Log(log);
    }
}