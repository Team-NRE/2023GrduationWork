using System.Collections;
using UnityEngine;
using Photon.Pun;
using Stat;

public class AmuletOfSteelStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _pv;
    protected PhotonView _playerPV;

    PlayerStats pStat;

    float shieldValue = default;
    float shieldRatioPerHealth = 0.4f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        effectTime = 5.0f;
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        shieldValue = pStat.maxHealth * shieldRatioPerHealth;
        _playerPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", shieldValue);
    }


    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            _playerPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", -shieldValue);
        }
    }
}
