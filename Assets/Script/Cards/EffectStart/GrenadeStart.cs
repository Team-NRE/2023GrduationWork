using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class GrenadeStart : MonoBehaviour
{
    ObjStats _Stats;
    PlayerStats _pStats;
    PlayerStats _EnemyStats;


    Transform Player = null;
    float Damage = default;
    float Debuff = default;
    int Enemylayer = default;
    float Save = 0;
    bool IsDebuff = false;

    float time = 0.0f;

    public void StartGrenade(Transform _Player, float _damage, LayerMask _enemylayer, float _debuff = default)
    {
        Player = _Player;
        Damage = _damage;
        Enemylayer = _enemylayer;
        Debuff = _debuff;
    }

    public void Start()
    {
        StartGrenade(Player, Damage, Enemylayer, Debuff);
    }

    public void Update()
    {
        if (IsDebuff == true)
        {
            time += Time.deltaTime;
            ManaRegenBack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                _Stats = other.gameObject.GetComponent<ObjStats>();
                _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _Stats.nowHealth -= (Damage + (_pStats.basicAttackPower * 0.5f));
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                _EnemyStats = other.gameObject.GetComponent<PlayerStats>();
                _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _EnemyStats.nowHealth -= (Damage + (_pStats.basicAttackPower * 0.5f));
                if (_EnemyStats.nowHealth <= 0) { _pStats.kill += 1; }


                if (Debuff != default)
                {
                    _EnemyStats.nowState = "Debuff";

                    Save = _EnemyStats.manaRegen;
                    _EnemyStats.manaRegen = 0;

                    IsDebuff = true;
                }
            }
        }
    }

    private void ManaRegenBack()
    {
        if (time >= Debuff - 0.02f || _EnemyStats.nowState == "Health")
        {
            _EnemyStats.manaRegen = Save;
            _EnemyStats.nowState = "Health";
            time = 0;
            IsDebuff = false;
        }
    }
}
