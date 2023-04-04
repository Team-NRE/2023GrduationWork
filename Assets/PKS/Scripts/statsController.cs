using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statsController : MonoBehaviour
{
    [Header ("- Stats")]
    [SerializeField] Stats stats;
    
    public float AttackProcessing(float damage)
    {
        return damage * stats.AttackPower * 0.01f;
    }

    public float HealthProcessing(float damage)
    {
        if (damage >= 0)
        {
            // 체력 회복
            float reducedHealth = stats.MaxHealth - stats.NowHealth;
            return (reducedHealth < damage) ? reducedHealth : damage;
        }
        else if (damage < 0)
        {
            // 체력 손상
        } 
        
        return 0;
    }
}
