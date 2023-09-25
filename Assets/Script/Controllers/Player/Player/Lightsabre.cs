using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public class Lightsabre : Players
{
    //리스폰 후 재설정
    public override void InitOnEnable()
    {
        //리스폰 지역 설정 (부활 유무)
        _agent.enabled = false;
        Transform respawn = (_pStats.isResurrection == false) ? GameObject.Find("CyborgRespawn").transform : this.transform;
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
        _pType = Define.PlayerType.Lightsabre;
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
        //살았을 때
        if (_pStats.nowHealth > 0)
        {
            if (_stopAttack == false)
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
                        _targetPV.RPC("photonStatSet", RpcTarget.All, "nowHealth" , -_pStats.basicAttackPower);
                    }

                    //타겟이 적 Player일 시
                    if (BaseCard._lockTarget.tag == "PLAYER")
                    {
                        _targetPV.RPC("photonStatSet", RpcTarget.All, "receviedDamage", _pStats.basicAttackPower);
                        if (_targetPV.GetComponent<PlayerStats>().nowHealth <= 0)
                            Managers.game.killEvent(_pv.ViewID, _targetPV.ViewID);
                    }
                }

                //움직임 초기화
                _agent.ResetPath();

                //평타 쿨타임
                _stopAttack = true;

                //애니메이션 Idle로 변환
                _state = Define.State.Idle;

                return;
            }
        }
        if (_pStats.nowHealth <= 0)
        {
            _state = Define.State.Die;
        }
    }


    //근접 공격 시 적 HP 관리
    //[PunRPC]
    //private void EnemyHPLog(string log)
    //{
    //    Debug.Log(log);
    //}
}
