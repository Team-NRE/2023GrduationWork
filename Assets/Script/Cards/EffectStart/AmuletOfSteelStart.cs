using Data;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteelStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

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
                //���������� ���� �ִ�ü�º��� ��ġ�� ü���� �־��ٸ�
                if (saveMaxhealth != default && saveNowhealth == default)
                {
                    //���� �ִ�ü������ ����
                    _pStats.maxHealth = saveMaxhealth;
                    //��ų �������� ���� ü���� ���� �ִ�ü�º��� ������ ���� �ִ�ü������ ����
                    if (saveMaxhealth <= _pStats.nowHealth) { _pStats.nowHealth = _pStats.maxHealth; }
                }

                //���� �ִ�ü�º��� �ȳ��ƴٸ� -> armor���� ��� �״��� ü�� ��ƾߵ�. 
                if (saveMaxhealth == default && saveNowhealth != default)
                {
                    if (saveNowhealth <= _pStats.nowHealth)
                    {
                        //�� �����ִٸ� ���� ����ü������ ���� 
                        _pStats.nowHealth = saveNowhealth;
                    }
                }

                start = false;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
