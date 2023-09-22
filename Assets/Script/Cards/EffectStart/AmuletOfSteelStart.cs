using Data;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteelStart : BaseEffect, IPunObservable
{
    PlayerStats _pStats;
    protected int _playerId;
    protected PhotonView _pv;

    float armor_Time = 0.01f;
    float effectTime = default;
    float saveMaxhealth = default;
    float saveNowhealth = default;

    bool start = false;

    public void StartAmulet(int playerId, float _effectTime, float _saveMaxhealth, float _saveNowhealth)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        _playerId = playerId;

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
                //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´ï¿½Ã¼ï¿½Âºï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ Ã¼ï¿½ï¿½ï¿½ï¿½ ï¿½Ö¾ï¿½ï¿½Ù¸ï¿½
                if (saveMaxhealth != default && saveNowhealth == default)
                {
                    //ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´ï¿½Ã¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                    _pStats.maxHealth = saveMaxhealth;
                    //ï¿½ï¿½Å³ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Ã¼ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´ï¿½Ã¼ï¿½Âºï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´ï¿½Ã¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
                    if (saveMaxhealth <= _pStats.nowHealth) { _pStats.nowHealth = _pStats.maxHealth; }
                }

                //ï¿½ï¿½ï¿½ï¿½ ï¿½Ö´ï¿½Ã¼ï¿½Âºï¿½ï¿½ï¿½ ï¿½È³ï¿½ï¿½Æ´Ù¸ï¿½ -> armorï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ ï¿½×´ï¿½ï¿½ï¿½ Ã¼ï¿½ï¿½ ï¿½ï¿½Æ¾ßµï¿½. 
                if (saveMaxhealth == default && saveNowhealth != default)
                {
                    if (saveNowhealth <= _pStats.nowHealth)
                    {
                        //ï¿½ï¿½î¸· ï¿½ï¿½ï¿½ï¿½ï¿½Ö´Ù¸ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ã¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ 
                        _pStats.nowHealth = saveNowhealth;
                    }
                }

                start = false;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ÀÚ½ÅÀÇ ·ÎÄÃ Ä³¸¯ÅÍÀÎ °æ¿ì ÀÚ½ÅÀÇ µ¥ÀÌÅÍ¸¦ ´Ù¸¥ ³×Æ®¿öÅ© À¯Àú¿¡°Ô ¼Û½Å
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
