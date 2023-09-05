using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
        _armorTime = 5.0f;
        
        _armorPercent = 50;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;
        _layer = layer;

        //띠로링
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);


        //방어막 이펙트
        Collider[] cols = Physics.OverlapSphere(_player.transform.position, _rangeScale, 1 << _layer);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "PLAYER")
            {
                Debug.Log(col.gameObject.name);
                PlayerStats _pStat = col.gameObject.GetComponent<PlayerStats>();

                //파티클
                //GameObject Wing = Managers.Resource.Instantiate($"Particle/Effect_AmuletOfSteel", col.transform);
                GameObject Wing = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_AmuletOfSteel", col.transform.position, Quaternion.identity);
                Wing.transform.localPosition = new Vector3(0, 1.12f, 0);

                //방어막
                float _armor = _pStat.maxHealth / 100 * _armorPercent;

                if( _pStat.nowHealth + _armor > _pStat.maxHealth )
                {
                    //피가 넘치면
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

                Wing.AddComponent<AmuletOfSteelStart>().StartAmulet(col.gameObject.name, _armorTime, saveMaxhealth, saveNowhealth);
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
