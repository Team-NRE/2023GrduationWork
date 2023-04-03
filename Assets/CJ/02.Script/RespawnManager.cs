using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
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
            gameManager.instance.player.nowHealth = gameManager.instance.player.maxHealth;
            RespawnTime = Respawn;
        }

        if (gameManager.instance.player.enabled == false)
        {
            RespawnTime -= Time.deltaTime;
        }
    }


}


