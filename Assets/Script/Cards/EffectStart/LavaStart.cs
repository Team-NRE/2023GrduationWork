using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class LavaStart : MonoBehaviour
{
    Transform Player = null;
    float Damage = default;
    int Enemylayer = default;


    public void StartLava(Transform _Player, float _damage, LayerMask _enemylayer)
    {
        Player = _Player;
        Damage = _damage;
        Enemylayer = _enemylayer;
    }

    public void Start()
    {
        StartLava(Player, Damage, Enemylayer);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats _Stats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _Stats.nowHealth -= (Damage + (_pStats.basicAttackPower * 0.01f));
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats _EnemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _EnemyStats.nowHealth -= (Damage + (_pStats.basicAttackPower * 0.01f));
            }
        }
    }
}
