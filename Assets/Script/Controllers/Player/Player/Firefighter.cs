using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public class Firefighter : Players
{
    //리스폰 후 재설정
    public override void InitOnEnable()
    {
        //리스폰 지역 설정 (부활 유무)
        _agent.enabled = false;
        Transform respawn = (_pStats.isResurrection == false) ? GameObject.Find("HumanRespawn").transform : this.transform;
        transform.position = respawn.position;
        _agent.enabled = true;

        //_state 설정 - 여기서 Idle 처리해야 애니메이션 오류 안남.
        _state = Define.State.Idle;

        base.InitOnEnable();
    }

    //Awake 초기화
    public override void Init()
    {
        base.Init();
        //스텟 호출
        _pType = Define.PlayerType.Firefighter;
        if (_pv.IsMine)
            _pv.RPC(
                "PlayerStatSetting",
                RpcTarget.All,
                _pType.ToString(),
                PhotonNetwork.LocalPlayer.NickName
            );
    }

    //Attack
    protected override void UpdateAttack()
    {
        //Range Off
        _IsRange = false;
        _attackRange[4].SetActive(_IsRange);

        //평타 공격
        if (BaseCard._lockTarget != null)
        {
            PhotonView _targetPV = BaseCard._lockTarget.GetComponent<PhotonView>();

            //타겟이 미니언, 타워일 시 
            if (BaseCard._lockTarget.tag != "PLAYER")
            {
                _targetPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", -_pStats.basicAttackPower);
            }

            //타겟이 적 Player일 시
            if (BaseCard._lockTarget.tag == "PLAYER")
            {
                _targetPV.RPC("photonStatSet", RpcTarget.All, _pv.ViewID, "receviedDamage", _pStats.basicAttackPower);
            }
        }
        //데미지 한번만 들어가기 위해
        _stopAttack = true;

        return;
    }
}
