using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralMobController : MonoBehaviour
{
    [SerializeField]
    float recognitionRange = 10.0f;

    [SerializeField]
    Collider[] nearbyEnemy;

    private void Update()
    {
        GetNearbyEnemy();
    }

    void GetNearbyEnemy()
    {
        nearbyEnemy = Physics.OverlapSphere(
            transform.position, // 현재 위치
            recognitionRange, // 인식 범위
            1 << (LayerMask.NameToLayer("Human")) | 1 << (LayerMask.NameToLayer("Cyborg"))
        );
    }
}
