using Data;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteelStart : BaseEffect
{
    PlayerStats _pStats;

    float armor_Time = 0.01f;
    float effectTime = default;
    float saveMaxhealth = default;
    float saveNowhealth = default;


    bool start = false;

    public void StartAmulet(int playerId, float _effectTime, float _saveMaxhealth, float _saveNowhealth)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();

        _pStats = Managers.game.RemoteTargetFinder(playerId).gameObject.GetComponent<PlayerStats>();
        effectTime = _effectTime;
        saveMaxhealth = _saveMaxhealth;
        saveNowhealth = _saveNowhealth;

        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (start == true)
        {
            armor_Time += Time.deltaTime;

            if (armor_Time >= effectTime - 0.01f)
            {
                //시전했을때 기존 최대체력보다 넘치는 체력이 있었다면
                if (saveMaxhealth != default && saveNowhealth == default)
                {
                    //기존 최대체력으로 복귀
                    _pStats.maxHealth = saveMaxhealth;
                    //스킬 끝날때도 현재 체력이 기존 최대체력보다 높으면 기존 최대체력으로 복귀
                    if (saveMaxhealth <= _pStats.nowHealth) { _pStats.nowHealth = _pStats.maxHealth; }
                }

                //기존 최대체력보다 안넘쳤다면 -> armor부터 깎고 그다음 체력 깎아야됨. 
                if (saveMaxhealth == default && saveNowhealth != default)
                {
                    if (saveNowhealth <= _pStats.nowHealth)
                    {
                        //방어막 남아있다면 기존 현재체력으로 복귀 
                        _pStats.nowHealth = saveNowhealth;
                    }
                }

                start = false;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
