using UnityEngine;

public partial class PlayerManager
{
    #region PlayerStats
    [Header("---Stats---")]
    [Header("- 공격 분야")]
    public float attackPower;
    public float absoluteAttackPower;
    public float attackSpeed;
    public float attackRange;
    public float criticalChance;
    public float criticalPower;

    [Header("- 방어 분야")]
    public float maxHealth;
    public float nowHealth;
    public float healthRegeneration;
    public float defensePower;

    [Header("- 버프 분야")]
    public float protectiveShield;
    public float protectiveShieldTime;

    [Header("- 디버프 분야")]
    public float moveSpeedReduction;
    public float moveSpeedReductionTime;
    public float skillSilenceTime;
    public float poisonDamage;
    public float poisonDamageTime;

    [Header("- 기타")]
    public float NowMana;
    public float ManaRegenerationTime;
    public float MaxMana = 3;
    public float level;

    public float speed;
    public float angularSpeed;
    public float acceleration;

    public float experience;
    public float globalToken;
    public float rangeToken;
    public float resource;
    public float resourceRange;
    public float recognitionRange;
    #endregion
    
    //스텟 읽기
    public float GetStats(string variableName)
    {
        if (variableName == "attackPower" || variableName == "공격력") return attackPower;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") return absoluteAttackPower;
        if (variableName == "attackSpeed" || variableName == "공격속도") return attackSpeed;
        if (variableName == "attackRange" || variableName == "공격범위") return attackRange;
        if (variableName == "criticalChance" || variableName == "크리확률") return criticalChance;
        if (variableName == "criticalPower" || variableName == "크리공격력") return criticalPower;

        if (variableName == "maxHealth" || variableName == "최대 체력") return maxHealth;
        if (variableName == "nowHealth" || variableName == "현재 체력") return nowHealth;
        if (variableName == "healthRegeneration" || variableName == "체력재생") return healthRegeneration;
        if (variableName == "defensePower" || variableName == "방어력") return defensePower;

        if (variableName == "protectiveShield" || variableName == "방어막") return protectiveShield;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") return protectiveShieldTime;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") return moveSpeedReduction;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") return moveSpeedReductionTime;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") return skillSilenceTime;
        if (variableName == "poisonDamage" || variableName == "독데미지") return poisonDamage;
        if (variableName == "poisonDamageTime" || variableName == "독시간") return poisonDamageTime;

        if (variableName == "mana" || variableName == "현재 마나") return NowMana;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") return ManaRegenerationTime;
        if (variableName == "maxmana" || variableName == "최대 마나") return MaxMana;
        if (variableName == "level" || variableName == "레벨") return level;
        if (variableName == "experience" || variableName == "경험치") return experience;
        if (variableName == "Speed" || variableName == "이동속도") return speed;
        if (variableName == "Angular Speed" || variableName == "회전속도") return angularSpeed;
        if (variableName == "globalToken" || variableName == "전역골드") return globalToken;
        if (variableName == "rangeToken" || variableName == "범위골드") return rangeToken;
        if (variableName == "resource" || variableName == "자원") return resource;
        if (variableName == "resourceRange" || variableName == "획득범위") return resourceRange;
        if (variableName == "recognitionRange" || variableName == "인식범위") return recognitionRange;

        return 0;
    }

    //스텟 쓰기
    public void SetStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력") attackPower = value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") absoluteAttackPower = value;
        if (variableName == "attackSpeed" || variableName == "공격속도") attackSpeed = value;
        if (variableName == "attackRange" || variableName == "공격범위") attackRange = value;
        if (variableName == "criticalChance" || variableName == "크리확률") criticalChance = value;
        if (variableName == "criticalPower" || variableName == "크리공격력") criticalPower = value;

        if (variableName == "maxHealth" || variableName == "최대 체력") maxHealth = value;
        if (variableName == "nowHealth" || variableName == "현재 체력") nowHealth = value;
        if (variableName == "healthRegeneration" || variableName == "체력재생") healthRegeneration = value;
        if (variableName == "defensePower" || variableName == "방어력") defensePower = value;

        if (variableName == "protectiveShield" || variableName == "방어막") protectiveShield = value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") protectiveShieldTime = value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") moveSpeedReduction = value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime = value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") skillSilenceTime = value;
        if (variableName == "poisonDamage" || variableName == "독데미지") poisonDamage = value;
        if (variableName == "poisonDamageTime" || variableName == "독시간") poisonDamageTime = value;

        if (variableName == "mana" || variableName == "현재 마나") NowMana = value;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") ManaRegenerationTime = value;
        if (variableName == "maxmana" || variableName == "최대 마나") MaxMana = value;
        if (variableName == "level" || variableName == "레벨") level = value;
        if (variableName == "experience" || variableName == "경험치") experience = value;
        if (variableName == "Speed" || variableName == "이동속도") speed = value;
        if (variableName == "Acceleration" || variableName == "가속도") acceleration = value;
        if (variableName == "Angular Speed" || variableName == "회전속도") angularSpeed = value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken = value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken = value;
        if (variableName == "resource" || variableName == "자원") resource = value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange = value;
    }

    //스텟 추가 / 감소
    public void AddStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력") attackPower += value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") absoluteAttackPower += value;
        if (variableName == "attackSpeed" || variableName == "공격속도") attackSpeed += value;
        if (variableName == "attackRange" || variableName == "공격범위") attackRange += value;
        if (variableName == "criticalChance" || variableName == "크리확률") criticalChance += value;
        if (variableName == "criticalPower" || variableName == "크리공격력") criticalPower += value;

        if (variableName == "maxHealth" || variableName == "최대 체력") maxHealth += value;
        if (variableName == "nowHealth" || variableName == "현재 체력") nowHealth += value;
        if (variableName == "healthRegeneration" || variableName == "체력재생") healthRegeneration += value;
        if (variableName == "defensePower" || variableName == "방어력") defensePower += value;

        if (variableName == "protectiveShield" || variableName == "방어막") protectiveShield += value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") protectiveShieldTime += value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") moveSpeedReduction += value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime += value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") skillSilenceTime += value;
        if (variableName == "poisonDamage" || variableName == "독데미지") poisonDamage += value;
        if (variableName == "poisonDamageTime" || variableName == "독시간") poisonDamageTime += value;

        if (variableName == "mana" || variableName == "현재 마나") NowMana -= 4 * value;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") ManaRegenerationTime += value;
        if (variableName == "maxmana" || variableName == "최대 마나") MaxMana += value;
        if (variableName == "level" || variableName == "레벨") level += value;
        if (variableName == "experience" || variableName == "경험치") experience += value;
        if (variableName == "Speed" || variableName == "이동속도") speed += value;
        if (variableName == "Angular Speed" || variableName == "회전속도") angularSpeed += value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken += value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken += value;
        if (variableName == "resource" || variableName == "자원") resource += value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange += value;
    }

}
