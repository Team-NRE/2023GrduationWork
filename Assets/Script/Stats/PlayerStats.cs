using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStats : MonoBehaviour
{
    #region PlayerStats

    [Header("-- 공격 --")]
    public float _basicAttackPower; //평타 공격력
    public float _attackSpeed; //평타 공속
    public float _attackRange; //평타 범위 
    //public GameObject _range; //평타 사거리 오브젝트


    [Header("-- 방어 --")]
    public float _maxHealth; //최대 체력
    public float _nowHealth; //현재 체력
    public float _healthRegeneration; //체력 재생량
    public float _defensePower; //방어력


    [Header("-- 마나 --")]
    public float _nowMana; //현재 마나
    public float _manaRegenerationTime; //마나 회복 속도
    public float _maxMana; //최대 마나


    [Header("-- 레벨 --")]
    public int _level; //레벨
    public float _experience; //경험치


    [Header("-- 이동 --")]
    public float _speed; //이동 속도
    #endregion


    //공격
    public float basicAttackPower { get { return _basicAttackPower; } set { _basicAttackPower = value; } }
    public float attackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public float attackRange { get { return _attackRange; } set { _attackRange = value; } }
    //public GameObject range { get { return _range; } set { _range = value; } }


    //방어
    public float maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public float nowHealth
    {
        get { return _nowHealth; }
        set
        {
            _nowHealth = value;
            if (_nowHealth >= _maxHealth) { _nowHealth = _maxHealth; }
        }
    }
    public float healthRegeneration { get { return _healthRegeneration; } set { _healthRegeneration = value; } }
    public float defensePower { get { return _defensePower; } set { _defensePower = value; } }


    //마나
    public float maxMana { get { return _maxMana; } set { _maxMana = value; } }
    public float nowMana
    {
        get { return _nowMana; }
        set
        {
            _nowMana += value;
            if (_nowMana >= _maxMana * _manaRegenerationTime) { _nowMana = _maxMana * _manaRegenerationTime; }
            if (_nowMana <= 0 ) {_nowMana = 0;}
        }
    }
    public float manaRegenerationTime
    {
        get { return _manaRegenerationTime; }
        set
        {
            _manaRegenerationTime = value;
        }
    }


    //레벨
    public int level { get { return _level; } set { _level = value; } }
    public float experience { get { return _experience; } set { _experience = value; } }


    //이동
    public float speed
    {
        get { return agent.speed; }
        set
        {
            _speed = value;
            agent.speed = _speed;
        }
    }

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        //공격
        basicAttackPower = 30.0f;
        attackRange = 6.0f;

        //방어
        maxHealth = 150.0f;
        nowHealth = maxHealth;

        //마나
        maxMana = 3.0f;
        manaRegenerationTime = 4.0f;

        //레벨
        level = 7;

        //이동
        speed = 4.0f;
    }

    private void Update()
    {
        nowMana = Time.deltaTime;
    }
}
