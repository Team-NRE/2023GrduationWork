using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class RespawnManager : MonoBehaviour
{
    private PlayerStats _pStats;
    private PlayerController _pController;

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
                _pStats.nowHealth = _pStats.maxHealth;
                _SetRespawn = RespawnTime;
                _pController.enabled = true;
            }
        }
    }


    //start
    public void Init()
    {
        GameObject _root = GameObject.Find("@Respawn");
        GameObject _player = GameObject.Find("PlayerController");
        if (_root == null)
        {
            _root = new GameObject { name = "@Respawn" };
            Object.DontDestroyOnLoad(_root);

            _pController = _player.GetComponent<PlayerController>();
            _pStats = _player.GetComponent<PlayerStats>();

            RespawnTime = 6.0f;
        }
    }

    //update
    public void Update_Init()
    {
        if (_pController.enabled == false)
        {
            SetRespawn = Time.deltaTime;
        }
    }

    public void Clear()
    {

    }
}


