using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    private EffectManager Instance = new EffectManager();
    private PhotonView pv;

    [Header("스킬 1번")]
    [SerializeField]    private GameObject expEffect;
    
    [Header("스킬 2번")]
    [SerializeField]    private GameObject expEffect2;


    private Transform tr;

    private void Start()
    {
        pv = GetComponent<PhotonView>();   
    }

    private void Update()
    {
        ExpSkill();
    }

    private void ExpSkill()
    {
        if (pv.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            tr = this.gameObject.transform;
            Instance.ExplosiveEffect(expEffect, tr, 1.0f);
        }
    }
}
