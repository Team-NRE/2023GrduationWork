using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    #region 변수 목록
    [Header ("- 공격 분야")]
    [SerializeField] float attackPower;
    [SerializeField] float absoluteAttackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalPower;

    [Header ("- 방어 분야")]
    [SerializeField] float maxHealth;
    [SerializeField] float nowHealth;
    [SerializeField] float healthRegeneration;
    [SerializeField] float defensePower;

    [Header ("- 버프 분야")]
    [SerializeField] float protectiveShield;
    [SerializeField] float protectiveShieldTime;
    
    [Header ("- 디버프 분야")]
    [SerializeField] float moveSpeedReduction;
    [SerializeField] float moveSpeedReductionTime;
    [SerializeField] float skillSilenceTime;
    [SerializeField] float poisonDamage;
    [SerializeField] float poisonDamageTime;

    [Header ("- 기타")]
    [SerializeField] float level;
    [SerializeField] float experience;
    [SerializeField] float moveSpeed;
    [SerializeField] float globalToken;
    [SerializeField] float rangeToken;
    [SerializeField] float resource;
    [SerializeField] float resourceRange;
    [SerializeField] float recognitionRange;
    #endregion

    void Start()
    {
        nowHealth = maxHealth;
        recognitionRange = attackRange * 5.0f;
    }

    public float GetStats(string variableName)
    {
        if (variableName == "attackPower" || variableName == "공격력")                      return attackPower;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력")          return absoluteAttackPower;
        if (variableName == "attackSpeed" || variableName == "공격속도")                    return attackSpeed;
        if (variableName == "attackRange" || variableName == "공격범위")                    return attackRange;
        if (variableName == "criticalChance" || variableName == "크리확률")                 return criticalChance;
        if (variableName == "criticalPower" || variableName == "크리공격력")                return criticalPower;

        if (variableName == "maxHealth" || variableName == "최대 체력")                     return maxHealth;
        if (variableName == "nowHealth" || variableName == "현재 체력")                     return nowHealth;
        if (variableName == "healthRegeneration" || variableName == "체력재생")             return healthRegeneration;
        if (variableName == "defensePower" || variableName == "방어력")                     return defensePower;

        if (variableName == "protectiveShield" || variableName == "방어막")                 return protectiveShield;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간")         return protectiveShieldTime;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트")   return moveSpeedReduction;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") return moveSpeedReductionTime;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간")           return skillSilenceTime;
        if (variableName == "poisonDamage" || variableName == "독데미지")                   return poisonDamage;
        if (variableName == "poisonDamageTime" || variableName == "독시간")                 return poisonDamageTime;

        if (variableName == "level" || variableName == "레벨")                              return level;
        if (variableName == "experience" || variableName == "경험치")                       return experience;
        if (variableName == "moveSpeed" || variableName == "이동속도")                      return moveSpeed;
        if (variableName == "globalToken" || variableName == "전역골드")                    return globalToken;
        if (variableName == "rangeToken" || variableName == "범위골드")                     return rangeToken;
        if (variableName == "resource" || variableName == "자원")                           return resource;
        if (variableName == "resourceRange" || variableName == "획득범위")                  return resourceRange;

        return 0;
    }

    public void SetStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력")                      attackPower             = value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력")          absoluteAttackPower     = value;
        if (variableName == "attackSpeed" || variableName == "공격속도")                    attackSpeed             = value;
        if (variableName == "attackRange" || variableName == "공격범위")                    attackRange             = value;
        if (variableName == "criticalChance" || variableName == "크리확률")                 criticalChance          = value;
        if (variableName == "criticalPower" || variableName == "크리공격력")                criticalPower           = value;

        if (variableName == "maxHealth" || variableName == "최대 체력")                     maxHealth               = value;
        if (variableName == "nowHealth" || variableName == "현재 체력")                     nowHealth               = value;
        if (variableName == "healthRegeneration" || variableName == "체력재생")             healthRegeneration      = value;
        if (variableName == "defensePower" || variableName == "방어력")                     defensePower            = value;

        if (variableName == "protectiveShield" || variableName == "방어막")                 protectiveShield        = value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간")         protectiveShieldTime    = value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트")   moveSpeedReduction      = value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime  = value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간")           skillSilenceTime        = value;
        if (variableName == "poisonDamage" || variableName == "독데미지")                   poisonDamage            = value;
        if (variableName == "poisonDamageTime" || variableName == "독시간")                 poisonDamageTime        = value;

        if (variableName == "level" || variableName == "레벨")                              level                   = value;
        if (variableName == "experience" || variableName == "경험치")                       experience              = value;
        if (variableName == "moveSpeed" || variableName == "이동속도")                      moveSpeed               = value;
        if (variableName == "globalToken" || variableName == "전역골드")                    globalToken             = value;
        if (variableName == "rangeToken" || variableName == "범위골드")                     rangeToken              = value;
        if (variableName == "resource" || variableName == "자원")                           resource                = value;
        if (variableName == "resourceRange" || variableName == "획득범위")                  resourceRange           = value;
    }

    public void AddStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력")                      attackPower             += value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력")          absoluteAttackPower     += value;
        if (variableName == "attackSpeed" || variableName == "공격속도")                    attackSpeed             += value;
        if (variableName == "attackRange" || variableName == "공격범위")                    attackRange             += value;
        if (variableName == "criticalChance" || variableName == "크리확률")                 criticalChance          += value;
        if (variableName == "criticalPower" || variableName == "크리공격력")                criticalPower           += value;

        if (variableName == "maxHealth" || variableName == "최대 체력")                     maxHealth               += value;
        if (variableName == "nowHealth" || variableName == "현재 체력")                     nowHealth               += value;
        if (variableName == "healthRegeneration" || variableName == "체력재생")             healthRegeneration      += value;
        if (variableName == "defensePower" || variableName == "방어력")                     defensePower            += value;

        if (variableName == "protectiveShield" || variableName == "방어막")                 protectiveShield        += value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간")         protectiveShieldTime    += value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트")   moveSpeedReduction      += value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime  += value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간")           skillSilenceTime        += value;
        if (variableName == "poisonDamage" || variableName == "독데미지")                   poisonDamage            += value;
        if (variableName == "poisonDamageTime" || variableName == "독시간")                 poisonDamageTime        += value;

        if (variableName == "level" || variableName == "레벨")                              level                   += value;
        if (variableName == "experience" || variableName == "경험치")                       experience              += value;
        if (variableName == "moveSpeed" || variableName == "이동속도")                      moveSpeed               += value;
        if (variableName == "globalToken" || variableName == "전역골드")                    globalToken             += value;
        if (variableName == "rangeToken" || variableName == "범위골드")                     rangeToken              += value;
        if (variableName == "resource" || variableName == "자원")                           resource                += value;
        if (variableName == "resourceRange" || variableName == "획득범위")                  resourceRange           += value;
    }
}
