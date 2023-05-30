/// ksPark
/// 
/// Object Stats Script

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Define;
//using UnityEditor.Animations;

namespace Stat
{
    public class ObjStats : MonoBehaviour
    {
        #region ObjStats

        [Header("-- 공격 --")]
        [SerializeField] private float _basicAttackPower; //평타 공격력
        [SerializeField] private float _attackSpeed; //평타 공속
        [SerializeField] private float _attackRange; //평타 범위 
        [SerializeField] private float _recognitionRange; //인식 범위


        [Header("-- 방어 --")]
        [SerializeField] private float _maxHealth; //최대 체력
        [SerializeField] private float _nowHealth; //현재 체력
        [SerializeField] private float _defensePower; //방어력
        [SerializeField] private float _nowBattery; // 현재 배터리
        
        [Header("-- 이동 --")]
        [SerializeField] private float _speed; //이동 속도

        [Header("-- 자원 --")]
        [SerializeField] private float _gold; //레벨
        [SerializeField] private float _experience; //경험치


        #endregion


        //공격
        public float basicAttackPower { get { return _basicAttackPower; } set { _basicAttackPower = value; } }
        public float attackSpeed { 
            get { return _attackSpeed; } 
            set 
            { 
                _attackSpeed = value; 
                GetComponent<Animator>().SetFloat("attackSpeed", _attackSpeed/(1+_attackSpeed));
            } 
        }

        public float attackRange { get { return _attackRange; } set { _attackRange = value; } }
        public float recognitionRange { get { return _recognitionRange; } set { _recognitionRange = value; } }

        //방어
        public float maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _nowHealth += value - _maxHealth;
                _maxHealth = value;
            }
        }
        
        public float nowHealth
        {
            get { return _nowHealth; }
            set
            {
                if (value > 0)
                {
                    _nowHealth = value;
                }
                else if (value < 0) 
                {
                    value *= 100 / (100 + defensePower);
                    _nowHealth = value;
                }

                if (_nowHealth >= _maxHealth) _nowHealth = _maxHealth;
                if (_nowHealth < 0) _nowHealth = 0;
            }
        }
        public float defensePower { get { return _defensePower; } set { _defensePower = value; } }
        public float nowBattery { get { return _nowBattery; } set { _nowBattery = value; } }

        //레벨
        public float gold { get { return _gold; } set { _gold = value; } }
        public float experience { get { return _experience; } set { _experience = value; } }


        //이동
        public float speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (GetComponent<NavMeshAgent>())
                    GetComponent<NavMeshAgent>().speed = _speed;
                GetComponent<Animator>().SetFloat("moveSpeed", _speed/(1+_speed));
            }
        }

        public void InitStatSetting(ObjectType type)
        {
            Dictionary<string, Data.ObjStat> dict = Managers.Data.ObjStatDict;
            Data.ObjStat stat = dict[type.ToString()];

            basicAttackPower = stat.basicAttackPower;
            attackSpeed = stat.attackSpeed;
            attackRange = stat.attackRange;
            recognitionRange = stat.recognitionRange;

            maxHealth = stat.maxHealth;
            nowHealth = maxHealth;
            defensePower = stat.defensePower;
            nowBattery = stat.nowBattery;

            speed = stat.speed;

            gold = stat.gold;
            experience = stat.experience;
        }
    }
}
