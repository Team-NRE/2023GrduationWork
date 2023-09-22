using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ö����
public class Card_AmuletOfSteel : UI_Card
{
    int _layer = default;

    float _armorTime = default;
    float _armorPercent = default;

    float saveMaxhealth = default;
    float saveNowhealth = default;
    
    public override void Init()
    {
        _cardBuyCost = 2000;
        _cost = 2;

        _rangeScale = 3.6f;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
        _armorTime = 5.0f;
        
        _armorPercent = 50;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);
        _layer = layer;

        //��θ�?
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield", this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        //���?����Ʈ
        Collider[] cols = Physics.OverlapSphere(_player.transform.position, _rangeScale, 1 << _layer);
        foreach (Collider col in cols)
        {
            GameObject remoteCol = Managers.game.RemoteTargetFinder(col.GetComponent<PhotonView>().ViewID);
            if (col.gameObject.tag == "PLAYER")
            {
                PlayerStats _pStat = remoteCol.gameObject.GetComponent<PlayerStats>();

                //��ƼŬ
                //GameObject Wing = Managers.Resource.Instantiate($"Particle/Effect_AmuletOfSteel", col.transform);

                GameObject Wing = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_AmuletofSteel", col.transform.position, Quaternion.Euler(-90, 0, 0));
                Wing.transform.parent = remoteCol.transform;
                Wing.transform.localPosition = new Vector3(0, 1.12f, 0);

                //���?
                float _armor = _pStat.maxHealth / 100 * _armorPercent;

                if( _pStat.nowHealth + _armor > _pStat.maxHealth )
                {
                    //�ǰ� ��ġ��
                    float overHealth = _pStat.nowHealth + _armor - _pStat.maxHealth;
                    saveMaxhealth = _pStat.maxHealth;
                    _pStat.maxHealth += overHealth;
                    _pStat.nowHealth = _pStat.maxHealth;
                }
                
                else if(_pStat.nowHealth + _armor <= _pStat.maxHealth)
                {
                    saveNowhealth = _pStat.nowHealth;
                    _pStat.nowHealth += _armor;
                }

                //Wing.GetComponent<AmuletOfSteelStart>().StartAmulet(col.gameObject.GetComponent<PhotonView>().ViewID, _armorTime, saveMaxhealth, saveNowhealth);
                Wing.GetComponent<AmuletOfSteelStart>().CardEffectInit(col.gameObject.GetComponent<PhotonView>().ViewID);
            }

            else continue;
        }

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
