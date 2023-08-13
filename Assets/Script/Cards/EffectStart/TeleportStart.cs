using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class TeleportStart : MonoBehaviour
{
    PlayerStats enemyStats;

    GameObject player = null;
    float damage = default;
    int enemylayer = default;
    float debuff = default;
    
    float saveMana = 0;
    bool isDebuff = false;

    float time = 0.0f;

    public void StartTeleport(string _player, float _damage, LayerMask _enemylayer, float _debuff = default)
    {
        player = GameObject.Find(_player);
        damage = _damage;
        enemylayer = _enemylayer;
    }

    public void Update()
    {

    }
}
