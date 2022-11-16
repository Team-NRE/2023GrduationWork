using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsForTest : MonoBehaviour
{
    #region 변수 목록
    [Header("- 공격 분야")]
    [SerializeField] float attackPower;
    [SerializeField] float absoluteAttackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalPower;

    [Header("- 방어 분야")]
    [SerializeField] float maxHealth;
    [SerializeField] float nowHealth;
    [SerializeField] float healthRegeneration;
    [SerializeField] float defensePower;

    [Header("- 버프 분야")]
    [SerializeField] float protectiveShield;
    [SerializeField] float protectiveShieldTime;

    [Header("- 디버프 분야")]
    [SerializeField] float moveSpeedReduction;
    [SerializeField] float moveSpeedReductionTime;
    [SerializeField] float skillSilenceTime;
    [SerializeField] float poisonDamage;
    [SerializeField] float poisonDamageTime;

    [Header("- 기타")]
    [SerializeField] float NowMana;
    [SerializeField] float ManaRegenerationTime;
    [SerializeField] float MaxMana;
    [SerializeField] float level;
    [SerializeField] float experience;
    [SerializeField] float moveSpeed;
    [SerializeField] float globalToken;
    [SerializeField] float rangeToken;
    [SerializeField] float resource;
    [SerializeField] float resourceRange;
    [SerializeField] float recognitionRange;
    #endregion

    [Header("- 마나")]
    public List<GameObject> ManaList = new List<GameObject>();

    void Awake()
    {        
        SetStats("현재 마나",0);
        SetStats("마나회복시간",4);
        SetStats("최대 마나",3);


        //마나 플레이 SEtting
        Transform ManaUI = GameObject.Find("Mana").transform;
        for (int i = 0; i < MaxMana; i++)
        {
            GameObject ManaChild = ManaUI.GetChild(i).gameObject;
            ManaList.Add(ManaChild);
            
            ManaList[i].GetComponent<Slider>().minValue = GetStats("마나회복시간") * i;
            ManaList[i].GetComponent<Slider>().maxValue = GetStats("마나회복시간") * (i + 1);
        }
    }

    void Update()
    {
        ManaPlay();
    }

    public void ManaPlay()
    {
        NowMana += Time.deltaTime;
        
        for (int i = 0; i < MaxMana; i++)
        {
            ManaList[i].GetComponent<Slider>().value = Mathf.Lerp(NowMana, GetStats("마나회복시간") * (i + 1), Time.deltaTime);
        }

        if(NowMana >= GetStats("마나회복시간") * GetStats("최대 마나"))
        {
            NowMana = GetStats("마나회복시간") * GetStats("최대 마나");
        }

        else if (NowMana <= 0)
        {
            NowMana = 0;
        }
    }

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
        if (variableName == "moveSpeed" || variableName == "이동속도") return moveSpeed;
        if (variableName == "globalToken" || variableName == "전역골드") return globalToken;
        if (variableName == "rangeToken" || variableName == "범위골드") return rangeToken;
        if (variableName == "resource" || variableName == "자원") return resource;
        if (variableName == "resourceRange" || variableName == "획득범위") return resourceRange;
        if (variableName == "recognitionRange" || variableName == "인식범위") return recognitionRange;

        return 0;
    }

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
        if (variableName == "moveSpeed" || variableName == "이동속도") moveSpeed = value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken = value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken = value;
        if (variableName == "resource" || variableName == "자원") resource = value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange = value;
    }

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
        if (variableName == "moveSpeed" || variableName == "이동속도") moveSpeed += value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken += value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken += value;
        if (variableName == "resource" || variableName == "자원") resource += value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange += value;
    }


}
