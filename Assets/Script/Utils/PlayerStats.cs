using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Define;
using Photon.Pun;

namespace Stat
{
    public class PlayerStats : MonoBehaviour
    {
        #region PlayerStats
        [Header("-- 캐릭 정보 --")]
        [SerializeField] private string _nickname; // 닉네임
        [SerializeField] private string _character; // 캐릭터

        [Header("-- 공격 --")]
        [SerializeField] private float _basicAttackPower; //평타 공격력
        [SerializeField] private float _attackSpeed; //평타 공속
        [SerializeField] private float _attackDelay; //평타 딜레이
        [SerializeField] private float _attackRange; //평타 범위 
        [SerializeField] private string _attackType; //평타 타입
        [SerializeField] private float _kill; //킬


        [Header("-- 방어 --")]
        [SerializeField] private float _nowHealth; //현재 체력
        [SerializeField] private float _maxHealth; //최대 체력
        [SerializeField] private float _healthRegeneration; //체력 재생량
        [SerializeField] private float _defensePower; //방어력
        [SerializeField] private float _receviedDamage; //계산된 입은 데미지
        [SerializeField] private float _shield; //방어막
        [SerializeField] private float _firstShield; //초기 방어막
        [SerializeField] private float _death; //데스

        [Header("-- 레벨 --")]
        private float _nowlevel;
        [SerializeField] private float _level; //레벨
        [SerializeField] private float _experience; //경험치
        [SerializeField] private float _levelUpEX; //레벨업시 늘어나는 경험치 양
        [SerializeField] private float _levelUpAP; //레벨업시 늘어나는 평타 공격력
        [SerializeField] private float _levelUpAS; //레벨업시 늘어나는 평타 공속
        [SerializeField] private float _levelUpHP; //레벨업시 늘어나는 최대 체력
        [SerializeField] private float _levelUpHR; //레벨업시 늘어나는 체력 회복량
        [SerializeField] private float _levelUpDP; //레벨업시 늘어나는 방어력


        [Header("-- 이동 --")]
        [SerializeField] private float _speed; //이동 속도


        [Header("-- 현재 상태 --")]
        [SerializeField] private string _nowState;
        [SerializeField] private bool _isResurrection;

        [Header("-- 진영 --")]
        [SerializeField] private int _playerArea; //내 진영
        [SerializeField] private int _enemyArea; //상대방 진영

        [Header("-- 마나 --")]
        [SerializeField] private float _nowMana; //현재 마나
        [SerializeField] private float _manaRegen; //마나 회복 속도
        [SerializeField] private float _maxMana; //최대 마나


        [Header("-- 자원 --")]
        [SerializeField] private float _gold; //돈

        #endregion

        // 캐릭 정보
        public string nickname { get { return _nickname; } set { _nickname = value; } }
        public string character { get { return _character; } set { _character = value; } }


        //공격
        public float basicAttackPower { get { return _basicAttackPower; } set { _basicAttackPower = value; } }
        public float attackSpeed
        {
            get { return _attackSpeed; }
            set
            {
                _attackSpeed = value;
                
                //공격 속도 계산식 (attackDelay 초당 1회 공격)
                attackDelay = 1 / (1 + _attackSpeed);
            }
        }
        public float attackDelay { get { return _attackDelay; } set { _attackDelay = value; } }
        public float attackRange { get { return _attackRange; } set { _attackRange = value; } }
        public string attackType { get { return _attackType; } set { _attackType = value; } }
        public float kill { get { return _kill; } set { _kill = value; } }



        //방어
        public float nowHealth
        {
            get { return _nowHealth; }
            set
            {
                if (value <= 0)
                {
                    _nowHealth = 0;
                    return;
                }

                if (value > 0)
                {
                    _nowHealth = value;
                }

                if (_nowHealth >= maxHealth) _nowHealth = maxHealth;
            }
        }
        public float maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
            }
        }

        public float healthRegeneration { get { return _healthRegeneration; } set { _healthRegeneration = value; } }
        public float defensePower { get { return _defensePower; } set { _defensePower = value; } }
        public (int, float) receviedDamage
        {
            get { return default; }
            set
            {
                value.Item2 *= 100 / (100 + defensePower);
                if (shield == 0) { _nowHealth -= value.Item2; }
                if (shield > 0)
                {
                    //Debug.Log($"방어막 : {shield}");
                    //Debug.Log($"들어온 데미지 : {value.Item2}");
                    float nowshield = shield - value.Item2;
                    //Debug.Log($"남은 방어막 : {nowshield}");
                    if(nowshield <= 0)
                    {
                        //Debug.Log($"남은 방어막이 없다면 현재 체력 : {nowHealth}{nowshield}");
                        nowHealth += nowshield;
                        //Debug.Log($"남은 방어막이 없다면 남은 체력 : {nowHealth}");
                        shield = 0;
                    }
                    if(nowshield > 0)
                    {
                        shield = (nowshield);
                        //Debug.Log($"남은 방어막이 있다면 남은 방어막 : {shield}");
                    }
                }

                if (_nowHealth <= 0 && !GetComponent<BaseController>()._startDie) 
                {
                    Managers.game.killEvent(value.Item1, GetComponent<PhotonView>().ViewID);
                }
            }
        }
        public float shield 
        {
            get { return _shield; }
            set 
            {
                _shield = value;

                if(_shield <= 0)
                {
                    _shield = 0;
                    firstShield = 0;
                }
            }
        }
        public float firstShield { get { return _firstShield; } 
            set 
            { 
                _firstShield = value;

                if (_firstShield <= 0)
                {
                    _firstShield = 0;
                }
            } 
        }
        public float death { get { return _death; } set { _death = value; } }



        //레벨
        public float level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (_level != _nowlevel)
                {
                    PhotonView _pv = GetComponent<PhotonView>();
                    _pv.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", _levelUpAP);
                    _pv.RPC("photonStatSet", RpcTarget.All, "attackSpeed", _levelUpAS);
                    _pv.RPC("photonStatSet", RpcTarget.All, "maxHealth", _levelUpHP);
                    _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", _levelUpHP);
                    _pv.RPC("photonStatSet", RpcTarget.All, "healthRegeneration ", _levelUpHR);
                    _pv.RPC("photonStatSet", RpcTarget.All, "defensePower", _levelUpDP);
                    _pv.RPC("photonStatSet", RpcTarget.All, "_nowlevel", _level);
                }
            }
        }
        public float experience
        { 
            get { return _experience; }
            set
            {
                if (nowHealth <= 0) return;
                if (level == 10) return;

                _experience += value;
                if (_experience > levelUpEx)
                {
                    level += 1;
                    levelUpEx += 60;
         
                    _experience = 0;

                }
            }
        }
        public float levelUpEx { get { return _levelUpEX; } set { _levelUpEX = value; } }



        //이동
        public float speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (GetComponent<NavMeshAgent>())
                {
                    GetComponent<NavMeshAgent>().speed = _speed;
                    GetComponent<NavMeshAgent>().acceleration = 80.0f;
                    GetComponent<NavMeshAgent>().updateRotation = false;
                }
            }
        }



        //현재 상태
        public string nowState { get { return _nowState; } set { _nowState = value; } }
        public bool isResurrection { get { return _isResurrection; } set { _isResurrection = value; } }


        //진영
        public int playerArea
        {
            get { return _playerArea; }
            set
            {
                _playerArea = value;
                this.gameObject.layer = _playerArea;
            }
        }
        public int enemyArea { get { return _enemyArea; } set { _enemyArea = value; } }


        //마나
        public float nowMana
        {
            get { return _nowMana; }
            set
            {
                _nowMana = value;
                if (_nowMana >= maxMana) { _nowMana = maxMana; }
                if (_nowMana <= 0) { _nowMana = 0; }
            }
        }
        public float manaRegen { get { return _manaRegen; } set { _manaRegen = value; } }
        public float maxMana { get { return _maxMana; } set { _maxMana = value; } }
        //마나 사용
        public (bool, float) UseMana(string _key = null, UI_Card ui_card = null)
        {
            bool CanUseCard;

            if (_key != null) { ui_card = GameObject.Find(_key).GetComponentInChildren<UI_Card>(); }
            float cardValue = ui_card._cost;

            if (nowMana >= cardValue) { CanUseCard = true; }
            else { CanUseCard = false; }

            return (CanUseCard, cardValue);
        }


        //자원
        public float gold { get { return _gold; } set { _gold = value; } }


        [PunRPC]
        public void PlayerStatSetting(string type, string name)
        {
            Dictionary<string, Data.PlayerStat> dict = Managers.Data.PlayerStatDict;
            Data.PlayerStat stat = dict[type];

            // 캐릭 정보
            nickname = name;
            character = stat.type;

            //공격
            basicAttackPower = stat.basicAttackPower;
            attackSpeed = stat.attackSpeed;
            attackRange = stat.attackRange;
            attackType = stat.attackType;
            kill = 0;

            //방어
            maxHealth = stat.maxHealth;
            nowHealth = maxHealth;
            healthRegeneration = stat.healthRegeneration;
            defensePower = stat.defensePower;
            death = 0;

            //레벨
            level = stat.level;
            _nowlevel = level;
            experience = stat.experience;
            _levelUpEX = stat.levelUpEX;
            _levelUpAP = stat.levelUpAP;
            _levelUpAS = stat.levelUpAS;
            _levelUpHP = stat.levelUpHP;
            _levelUpHR = stat.levelUpHR;
            _levelUpDP = stat.levelUpDP;

            //이동
            speed = stat.speed;

            //현재 상태
            nowState = stat.nowState;
            isResurrection = false;

            //진영
            if (type == "Police" || type == "Firefighter")
            {
                playerArea = (int)Layer.Human;
                enemyArea = (int)Layer.Cyborg;
            }
            else
            {
                playerArea = (int)Layer.Cyborg;
                enemyArea = (int)Layer.Human;
            }

            //마나
            manaRegen = stat.manaRegen; //마나 회복 속도
            maxMana = stat.maxMana; //최대 마나
            nowMana = maxMana; //현재 마나

            //자원
            gold = stat.gold;
        }

        [PunRPC]
        public void photonStatSet(string statName, float value)
        {
            if (statName == "basicAttackPower")     basicAttackPower    += value;
            if (statName == "attackSpeed")          attackSpeed         += value;
            if (statName == "attackDelay")          attackDelay         += value;
            if (statName == "attackRange")          attackRange         += value;
            if (statName == "kill")                 kill                += value;
            if (statName == "nowHealth")            nowHealth           += value;
            if (statName == "maxHealth")            maxHealth           += value;
            if (statName == "healthRegeneration")   healthRegeneration  += value;
            if (statName == "defensePower")         defensePower        += value;
            if (statName == "shield")               shield              += value;
            if (statName == "firstShield")          firstShield         += value;
            if (statName == "_nowlevel")            _nowlevel           += value;
            if (statName == "experience")           experience          += value;
            if (statName == "speed")                speed               += value;
            if (statName == "manaRegen")            manaRegen           += value;
            if (statName == "maxMana")              maxMana             += value;
        }

        [PunRPC]
        public void photonStatSet(int attackID, string statName, float value)
        {
            if (statName == "receviedDamage")       receviedDamage      = (attackID, value);
        }
    }
}