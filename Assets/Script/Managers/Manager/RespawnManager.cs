using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;

public class RespawnManager : MonoBehaviour
{
    State pState;
    PlayerStats pStat;
    PlayerType pType;
    GameObject Player;

    Transform HumanRespawn;
    Transform CyborgRespawn;
    Transform RespawnPos;

    public float _RespawnTime;
    public float RespawnTime
    {
        get { return _RespawnTime; }
        set
        {
            _RespawnTime = value;
            _SetRespawn = _RespawnTime;
        }
    }


    public float _SetRespawn;
    public float SetRespawn
    {
        get { return _SetRespawn; }
        set
        {
            _SetRespawn -= value;
            if (_SetRespawn <= 0)
            {
                Player.GetComponent<BaseController>().enabled = true;
                Player.GetComponent<CapsuleCollider>().enabled = true;

                _SetRespawn = RespawnTime;
            }
        }
    }

    private void Start()
    {
        

        RespawnTime = 6.0f;
    }

    //update
    public void Update()
    {
        if (pStat == null)
        {
            pStat = Managers.game.myCharacter?.GetComponent<PlayerStats>();
            return;
        }
        
        // if (pStat.nowHealth <= 0)
        // {
        //     SetRespawn = Time.deltaTime;
        // }
    }
}
