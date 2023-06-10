using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Stat
{
    public class PlayerStats : MonoBehaviour
    {
        #region PlayerStats

        [Header("-- 공격 --")]
        public float _basicAttackPower; //평타 공격력
        public float _attackSpeed; //평타 공속
        public float _attackDelay; //평타 딜레이
        public float _attackRange; //평타 범위 


        [Header("-- 방어 --")]
        public float _maxHealth; //최대 체력
        public float _nowHealth; //현재 체력
        public float _healthRegeneration; //체력 재생량
        public float _defensePower; //방어력


        [Header("-- 레벨 --")]
        public int _level; //레벨
        public float _experience; //경험치


        [Header("-- 이동 --")]
        public float _speed; //이동 속도

        [Header("-- 진영 --")]
        public LayerMask _layerArea; //진영 레이어
        private int _playerArea; //내 진영
        public int _enemyArea; //상대방 진영

        #endregion


        //공격
        public float basicAttackPower { get { return _basicAttackPower; } set { _basicAttackPower = value; } }
        public float attackSpeed
        {
            get { return _attackSpeed; }
            set
            {
                _attackSpeed = value;
                //공격 속도 계산식
                _attackDelay = (1 / _attackSpeed);
            }
        }
        public float attackRange { get { return _attackRange; } set { _attackRange = value; } }


        //방어
        public float maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                _nowHealth = _maxHealth;
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

        public float healthRegeneration { get { return _healthRegeneration; } set { _healthRegeneration = value; } }
        public float defensePower { get { return _defensePower; } set { _defensePower = value; } }

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

        //진영
        public int playerArea
        {
            get
            {
                //Layer 번호 알아내기
                _playerArea = ((int)Mathf.Log(_layerArea.value, 2));
                return _playerArea;
            }
            set { _layerArea = value; }
        }

        public int enemyArea
        {
            get
            {
                //적 Layer 설정
                _enemyArea = (_playerArea == (int)Define.Layer.Human) ? (int)Define.Layer.Cyborg : (int)Define.Layer.Human;
                return _enemyArea;
            }
        }

        public Define.PlayerAttackType AttackType { get; set; } = Define.PlayerAttackType.Undefine;


        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            //agent setting
            agent.acceleration = 80.0f;
            agent.updateRotation = false;

            //공격
            basicAttackPower = 30.0f;
            attackSpeed = 0.8f;
            attackRange = 6.0f;

            //방어
            maxHealth = 300.0f;
            defensePower = 50.0f;

            //레벨
            level = 7;

            //이동
            speed = 4.0f;

            //진영
            //진영 선택 창에서 진영 정보를 불러와 area에 저장
            //area = 진영정보 불러오기 -> 일단 Inspector에서 선택.
            this.gameObject.layer = 6;

            //평타 타입
            AttackType = Define.PlayerAttackType.LongRange;
        }
    }
}
