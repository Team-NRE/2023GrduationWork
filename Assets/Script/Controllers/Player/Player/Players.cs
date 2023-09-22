using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public class Players : BaseController
{
    public override void Init()
    {
        base.Init();

        //초기화
        _pStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _pv = GetComponent<PhotonView>();
    }

    public override void InitOnEnable()
    {
        base.InitOnEnable();
    }
}
