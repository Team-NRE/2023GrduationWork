using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class IcePrisonStart : MonoBehaviour
{
    float effectTime;
    float startEffect = 0.01f;

    GameObject player = null;

    PlayerStats pStat;

    public void StartIcePrison(int _player, float _effectTime)
    {
        effectTime = _effectTime;

        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
        pStat = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if(startEffect > effectTime - 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
