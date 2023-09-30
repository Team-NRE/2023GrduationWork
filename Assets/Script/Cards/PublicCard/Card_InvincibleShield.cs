using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Data;
using Photon.Pun;

// 무적 방패
public class Card_InvincibleShield : UI_Card
{
    int _layer = default;
    float _invincibleTime = default;
    float _shieldTime = default;

    public override void Init()
    {
        _cardBuyCost = 3333;
        _cost = 3;
        //_defence = 10000;

        _rangeType = Define.CardType.None;
        _rangeScale = 3.6f;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
        _invincibleTime = 1.5f;
        _shieldTime = 3.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        _layer = layer;

        //띠로링
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield", ground, Quaternion.identity);
        //_effectObject.transform.parent = _player.transform;
        
        //_effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        //쉴드, 팀원들 찾아서 쉴드 이펙트 씌워주는 내용
        Collider[] cols = Physics.OverlapSphere(_player.transform.position, _rangeScale, 1 << _layer);
        foreach (Collider col in cols)
        {
            int userId = Managers.game.RemoteColliderId(col);
            //col.transform -> Police, 미니언
            GameObject shield = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield_1", col.transform.position, Quaternion.identity);
            shield.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, userId);

            if(col.gameObject.tag == "PLAYER")
            {
                PlayerStats _pStat = col.gameObject.GetComponent<PlayerStats>();
                _pStat.defensePower += 10000;
            }

            else if (col.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = col.gameObject.GetComponent<ObjStats>();
                oStats.defensePower += 10000;
            }
        }

        return _effectObject;
    }



    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
