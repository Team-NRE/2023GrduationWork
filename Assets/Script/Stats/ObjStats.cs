using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjStats : MonoBehaviour
{
    #region 변수 목록
    [Header ("- 공격 분야")]
    [SerializeField] float attackPower;             // 공격력
    [SerializeField] float absoluteAttackPower;     // 절대 공격력
    [SerializeField] float attackSpeed;             // 공격 속도
    [SerializeField] float attackCoolingTime;       // 공격 쿨타임
    [SerializeField] float attackRange;             // 공격 범위
    [SerializeField] float criticalChance;          // 크리 확률
    [SerializeField] float criticalPower;           // 크리 공격력

    [Header ("- 방어 분야")]
    [SerializeField] float maxHealth;               // 최대 체력
    [SerializeField] float nowHealth;               // 현재 체력
    [SerializeField] float healthRegeneration;      // 체력 재생
    [SerializeField] float defensePower;            // 방어력

    [Header ("- 버프 분야")]
    [SerializeField] float protectiveShield;        // 방어막
    [SerializeField] float protectiveShieldTime;    // 방어막 시간
    
    [Header ("- 디버프 분야")]
    [SerializeField] float moveSpeedReduction;      // 이동속도 감소 퍼센트
    [SerializeField] float moveSpeedReductionTime;  // 이동속도 감소 시간
    [SerializeField] float skillSilenceTime;        // 스킬 침묵 시간
    [SerializeField] float poisonDamage;            // 독 데미지
    [SerializeField] float poisonDamageTime;        // 독 시간

    [Header ("- 기타")]
    [SerializeField] float level;                   // 레벨
    [SerializeField] float experience;              // 경험치
    [SerializeField] float moveSpeed;               // 이동 속도
    [SerializeField] float globalToken;             // 전역 골드
    [SerializeField] float rangeToken;              // 범위 골드
    [SerializeField] float resource;                // 자원
    [SerializeField] float resourceRange;           // 자원 획득 범위
    [SerializeField] float recognitionRange;        // 인식 범위
    #endregion

    #region 공격 분야 get, set
    // 공격력
    public float AttackPower
    {
        get 
        {
            
            return attackPower; 
        }
        
        set 
        { 
            attackPower = value; 
        }
    }

    // 절대 공격력
    public float AbsoluteAttackPower
    {
        get 
        { 
            return absoluteAttackPower; 
        }
        
        set 
        { 
            absoluteAttackPower = value; 
        }
    }

    // 공격 속도
    public float AttackSpeed
    {   
        get 
        { 
            return attackSpeed; 
        }
        
        set 
        { 
            attackSpeed = value; 
        }
    }

    // 공격 쿨타임
    public float AttackCoolingTime
    {
        get 
        { 
            return attackCoolingTime; 
        }
        
        set 
        { 
            attackCoolingTime = value; 
        }
    }

    // 공격 범위
    public float AttackRange
    {
        get 
        { 
            return attackRange; 
        }
        
        set 
        { 
            attackRange = value; 
        }
    }

    // 크리티컬 확률
    public float CriticalChance
    {
        get 
        { 
            return criticalChance; 
        }
        
        set 
        { 
            criticalChance = value; 
        }
    }

    // 크리티컬 공격력
    public float CriticalPower
    {
        get 
        { 
            return criticalPower; 
        }
        
        set 
        { 
            criticalPower = value; 
        }
    }
    #endregion

    #region 방어 분야 get, set
    // 최대 체력
    public float MaxHealth
    {
        get 
        { 
            return maxHealth; 
        }
        
        set 
        { 
            maxHealth = value; 
        }
    }

    // 현재 체력
    public float NowHealth
    {
        get 
        { 
            return nowHealth; 
        }
        
        set 
        { 
            nowHealth = value; 
        }
    }

    // 체력 재생
    public float HealthRegeneration
    {
        get 
        { 
            return healthRegeneration; 
        }
        
        set 
        { 
            healthRegeneration = value;
        }
    }

    // 방어력
    public float DefensePower
    {
        get 
        { 
            return defensePower; 
        }
        
        set 
        { 
            defensePower = value; 
        }
    }
    #endregion

    #region 버프 분야 get, set
    // 방어막
    public float ProtectiveShield
    {
        get 
        { 
            return protectiveShield; 
        }
        
        set 
        { 
            protectiveShield = value; 
        }
    }

    // 방어막 시간
    public float ProtectiveShieldTime
    {
        get 
        { 
            return protectiveShieldTime; 
        }
        
        set 
        { 
            protectiveShieldTime = value; 
        }
    }
    #endregion

    #region 디버프 분야 get, set
    // 이동속도 감소 퍼센트
    public float MoveSpeedReduction
    {
        get 
        { 
            return moveSpeedReduction; 
        }
        
        set 
        { 
            moveSpeedReduction = value; 
        }
    }

    // 이동속도 감소 시간
    public float MoveSpeedReductionTime
    {
        get 
        { 
            return moveSpeedReductionTime; 
        }
        
        set 
        { 
            moveSpeedReductionTime = value; 
        }
    }

    // 스킬 침묵 시간
    public float SkillSilenceTime
    {
        get 
        { 
            return skillSilenceTime; 
        }
        
        set 
        { 
            skillSilenceTime = value; 
        }
    }

    // 독 데미지
    public float PoisonDamage
    {
        get 
        { 
            return poisonDamage; 
        }
        
        set 
        { 
            poisonDamage = value; 
        }
    }

    // 독 시간
    public float PoisonDamageTime
    {
        get 
        { 
            return poisonDamageTime; 
        }
        
        set 
        { 
            poisonDamageTime = value; 
        }
    }
    #endregion

    #region 기타 get, set
    // 레벨
    public float Level
    {
        get 
        { 
            return level; 
        }
        
        set 
        { 
            level = value; 
        }
    }

    // 경험치
    public float Experience
    {
        get 
        { 
            return experience; 
        }
        
        set 
        { 
            experience = value; 
        }
    }

    // 이동 속도
    public float MoveSpeed
    {
        get 
        { 
            return moveSpeed; 
        }
        
        set 
        { 
            moveSpeed = value; 
        }
    }

    // 전역 골드
    public float GlobalToken
    {
        get 
        { 
            return globalToken; 
        }
        
        set 
        { 
            globalToken = value; 
        }
    }

    // 범위 골드
    public float RangeToken
    {
        get 
        { 
            return rangeToken; 
        }
        
        set 
        { 
            rangeToken = value; 
        }
    }

    // 자원
    public float Resource
    {
        get 
        { 
            return resource; 
        }
        
        set 
        { 
            resource = value; 
        }
    }

    // 자원 획득 범위
    public float ResourceRange
    {
        get 
        { 
            return resourceRange; 
        }
        
        set 
        { 
            resourceRange = value; 
        }
    }

    // 인식 범위
    public float RecognitionRange
    {
        get 
        { 
            return recognitionRange; 
        }
        
        set 
        { 
            recognitionRange = value; 
        }
    }
    #endregion
}
