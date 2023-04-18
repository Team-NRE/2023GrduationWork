using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class RespawnManager : MonoBehaviour
{
    PlayerStats _pStat;

    public float SetRespawn;
    public float RespawnTime;
    public float Respawn
    {
        get { return SetRespawn; }
        set { value = SetRespawn; }
    }

    private void Start() 
    {
        RespawnTime = Respawn;
    }

    void Update()
    {
        RespawnStart();
    }

    void RespawnStart()
    {
        if (RespawnTime <= 0)
        {
            gameManager.instance.player.enabled = true;
            _pStat.nowHealth = _pStat.maxHealth;
            RespawnTime = Respawn;
        }

        if (gameManager.instance.player.enabled == false)
        {
            RespawnTime -= Time.deltaTime;
        }
    }
}


