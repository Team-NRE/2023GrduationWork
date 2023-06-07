using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class RespawnManager : MonoBehaviour
{
    private PlayerStats _pStats;
    private Police _pController;

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

    private void Start()
    {
        Init();
    }

    //start
    public void Init()
    {
        GameObject _player = GameObject.FindWithTag("PLAYER");

        _pController = _player.GetComponent<Police>();
        _pStats = _player.GetComponent<PlayerStats>();

        RespawnTime = 6.0f;
    }

    //update
    public void Update()
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