using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitStart : MonoBehaviour
{
    GameObject player = null;
    PlayerStats pStats;
    float healthRegen = default;
    int teamLayer = default;

    public void StartHealthKit(string _player, float _healthhRegen, int _teamLayer)
    {
        player = GameObject.Find(_player);
        healthRegen = _healthhRegen;
        teamLayer = _teamLayer;

        pStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        pStats.nowHealth += healthRegen;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == teamLayer && other.gameObject.tag != "PLAYER")
        {
            ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
            oStats.nowHealth += healthRegen;
        }
    }
}
