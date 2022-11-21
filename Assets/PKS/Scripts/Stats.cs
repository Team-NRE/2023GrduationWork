using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
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

    void Start()
    {
        // 기본 값 세팅
        nowHealth = maxHealth;
        attackCoolingTime = 1 / attackSpeed;
    }

    public float GetStats(string variableName)
    {
        /*
            스텟 값 리턴
        */

        if (variableName == "attackPower")                  return attackPower;
        if (variableName == "absoluteAttackPower")          return absoluteAttackPower;
        if (variableName == "attackSpeed")                  return attackSpeed;
        if (variableName == "attackCoolingTime")            return attackCoolingTime;
        if (variableName == "attackRange")                  return attackRange;
        if (variableName == "criticalChance")               return criticalChance;
        if (variableName == "criticalPower")                return criticalPower;

        if (variableName == "maxHealth")                    return maxHealth;
        if (variableName == "nowHealth")                    return nowHealth;
        if (variableName == "healthRegeneration")           return healthRegeneration;
        if (variableName == "defensePower")                 return defensePower;

        if (variableName == "protectiveShield")             return protectiveShield;
        if (variableName == "protectiveShieldTime")         return protectiveShieldTime;

        if (variableName == "moveSpeedReduction")           return moveSpeedReduction;
        if (variableName == "moveSpeedReductionTime")       return moveSpeedReductionTime;
        if (variableName == "skillSilenceTime")             return skillSilenceTime;
        if (variableName == "poisonDamage")                 return poisonDamage;
        if (variableName == "poisonDamageTime")             return poisonDamageTime;

        if (variableName == "level")                        return level;
        if (variableName == "experience")                   return experience;
        if (variableName == "moveSpeed")                    return moveSpeed;
        if (variableName == "globalToken")                  return globalToken;
        if (variableName == "rangeToken")                   return rangeToken;
        if (variableName == "resource")                     return resource;
        if (variableName == "resourceRange")                return resourceRange;
        if (variableName == "recognitionRange")             return recognitionRange;
        
        return float.NaN;
    }

    public void SetStats(string variableName, float value)
    {
        /*
            스텟 값 변경
        */
        
        if (variableName == "attackPower"               && !float.IsNaN(attackPower))               attackPower             = value;
        if (variableName == "absoluteAttackPower"       && !float.IsNaN(absoluteAttackPower))       absoluteAttackPower     = value;
        if (variableName == "attackSpeed"               && !float.IsNaN(attackSpeed))               attackSpeed             = value;
        if (variableName == "attackCoolingTime"         && !float.IsNaN(attackCoolingTime))         attackCoolingTime       = value;
        if (variableName == "attackRange"               && !float.IsNaN(attackRange))               attackRange             = value;
        if (variableName == "criticalChance"            && !float.IsNaN(criticalChance))            criticalChance          = value;
        if (variableName == "criticalPower"             && !float.IsNaN(criticalPower))             criticalPower           = value;

        if (variableName == "maxHealth"                 && !float.IsNaN(maxHealth))                 maxHealth               = value;
        if (variableName == "nowHealth"                 && !float.IsNaN(nowHealth))                 nowHealth               = value;
        if (variableName == "healthRegeneration"        && !float.IsNaN(healthRegeneration))        healthRegeneration      = value;
        if (variableName == "defensePower"              && !float.IsNaN(defensePower))              defensePower            = value;

        if (variableName == "protectiveShield"          && !float.IsNaN(protectiveShield))          protectiveShield        = value;
        if (variableName == "protectiveShieldTime"      && !float.IsNaN(protectiveShieldTime))      protectiveShieldTime    = value;

        if (variableName == "moveSpeedReduction"        && !float.IsNaN(moveSpeedReduction))        moveSpeedReduction      = value;
        if (variableName == "moveSpeedReductionTime"    && !float.IsNaN(moveSpeedReductionTime))    moveSpeedReductionTime  = value;
        if (variableName == "skillSilenceTime"          && !float.IsNaN(skillSilenceTime))          skillSilenceTime        = value;
        if (variableName == "poisonDamage"              && !float.IsNaN(poisonDamage))              poisonDamage            = value;
        if (variableName == "poisonDamageTime"          && !float.IsNaN(poisonDamageTime))          poisonDamageTime        = value;

        if (variableName == "level"                     && !float.IsNaN(level))                     level                   = value;
        if (variableName == "experience"                && !float.IsNaN(experience))                experience              = value;
        if (variableName == "moveSpeed"                 && !float.IsNaN(moveSpeed))                 moveSpeed               = value;
        if (variableName == "globalToken"               && !float.IsNaN(globalToken))               globalToken             = value;
        if (variableName == "rangeToken"                && !float.IsNaN(rangeToken))                rangeToken              = value;
        if (variableName == "resource"                  && !float.IsNaN(resource))                  resource                = value;
        if (variableName == "resourceRange"             && !float.IsNaN(resourceRange))             resourceRange           = value;
        if (variableName == "recognitionRange"          && !float.IsNaN(recognitionRange))          recognitionRange        = value;
    }

    public void AddStats(string variableName, float value)
    {
        /*
            스텟 값 증감
        */

        if (variableName == "attackPower"               && !float.IsNaN(attackPower))               attackPower             += value;
        if (variableName == "absoluteAttackPower"       && !float.IsNaN(absoluteAttackPower))       absoluteAttackPower     += value;
        if (variableName == "attackSpeed"               && !float.IsNaN(attackSpeed))               attackSpeed             += value;
        if (variableName == "attackCoolingTime"         && !float.IsNaN(attackCoolingTime))         attackCoolingTime       += value;
        if (variableName == "attackRange"               && !float.IsNaN(attackRange))               attackRange             += value;
        if (variableName == "criticalChance"            && !float.IsNaN(criticalChance))            criticalChance          += value;
        if (variableName == "criticalPower"             && !float.IsNaN(criticalPower))             criticalPower           += value;

        if (variableName == "maxHealth"                 && !float.IsNaN(maxHealth))                 maxHealth               += value;
        if (variableName == "nowHealth"                 && !float.IsNaN(nowHealth))                 nowHealth               += value;
        if (variableName == "healthRegeneration"        && !float.IsNaN(healthRegeneration))        healthRegeneration      += value;
        if (variableName == "defensePower"              && !float.IsNaN(defensePower))              defensePower            += value;

        if (variableName == "protectiveShield"          && !float.IsNaN(protectiveShield))          protectiveShield        += value;
        if (variableName == "protectiveShieldTime"      && !float.IsNaN(protectiveShieldTime))      protectiveShieldTime    += value;

        if (variableName == "moveSpeedReduction"        && !float.IsNaN(moveSpeedReduction))        moveSpeedReduction      += value;
        if (variableName == "moveSpeedReductionTime"    && !float.IsNaN(moveSpeedReductionTime))    moveSpeedReductionTime  += value;
        if (variableName == "skillSilenceTime"          && !float.IsNaN(skillSilenceTime))          skillSilenceTime        += value;
        if (variableName == "poisonDamage"              && !float.IsNaN(poisonDamage))              poisonDamage            += value;
        if (variableName == "poisonDamageTime"          && !float.IsNaN(poisonDamageTime))          poisonDamageTime        += value;

        if (variableName == "level"                     && !float.IsNaN(level))                     level                   += value;
        if (variableName == "experience"                && !float.IsNaN(experience))                experience              += value;
        if (variableName == "moveSpeed"                 && !float.IsNaN(moveSpeed))                 moveSpeed               += value;
        if (variableName == "globalToken"               && !float.IsNaN(globalToken))               globalToken             += value;
        if (variableName == "rangeToken"                && !float.IsNaN(rangeToken))                rangeToken              += value;
        if (variableName == "resource"                  && !float.IsNaN(resource))                  resource                += value;
        if (variableName == "resourceRange"             && !float.IsNaN(resourceRange))             resourceRange           += value;
        if (variableName == "recognitionRange"          && !float.IsNaN(recognitionRange))          recognitionRange        += value;
    }
}
