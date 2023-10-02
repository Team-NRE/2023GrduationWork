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


    void Start()
    {
        player = transform.parent.gameObject; 
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        effectTime = 5.0f;
        shieldValue = pStat.maxHealth * shieldRatioPerHealth;
        _playerPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", shieldValue);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            _playerPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", -shieldValue);

            Destroy(gameObject);
        }
    }
}
