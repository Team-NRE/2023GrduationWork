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
                Player.GetComponent<CapsuleCollider>().enabled = true;
                Player.GetComponent<BaseController>().enabled = true;

                Player.transform.position = default;
                Player.transform.position = RespawnPos.localPosition;
                Debug.Log(Player.transform.position);

                pStat.nowHealth = pStat.maxHealth;
                
                _SetRespawn = RespawnTime;
            }
        }
    }

    private void Start()
    {
        HumanRespawn = transform.GetChild(0);
        CyborgRespawn = transform.GetChild(1);
        pStatAction();

        RespawnTime = 6.0f;
    }

    public void pStatAction()
    {
        switch (pType)
        {
            case Define.PlayerType.Police:
                Player = GameObject.Find("Police");
                pStat = Player.GetComponent<PlayerStats>();
                RespawnPos = HumanRespawn;

                break;

            case Define.PlayerType.Firefighter:
                Player =  GameObject.Find("Firefight");
                pStat = Player.GetComponent<PlayerStats>();
                RespawnPos = HumanRespawn;

                break;

            case Define.PlayerType.Lightsaber:
                Player = GameObject.Find("Lightsaber");
                pStat = Player.GetComponent<PlayerStats>();
                RespawnPos = CyborgRespawn;

                break;

            case Define.PlayerType.Monk:
                Player = GameObject.Find("Monk");
                pStat = Player.GetComponent<PlayerStats>();
                RespawnPos = CyborgRespawn;
                
                break;
        }
    }

    //update
    public void Update()
    {
        return;
        
        if (pStat.nowHealth <= 0)
        {
            SetRespawn = Time.deltaTime;
        }
    }
}
