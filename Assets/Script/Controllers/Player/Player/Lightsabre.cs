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
        //평타 공격
        if (BaseCard._lockTarget != null && oneShot == false)
        {
            if (_pv.IsMine)
            {
                //평타 소리
                attackSound.GetComponent<AudioSource>().enabled = true;

                //데미지 피해
                _pv.RPC("ApplyDamage", RpcTarget.All, BaseCard._lockTarget.GetComponent<PhotonView>().ViewID);

                //한발만 쏘기
                oneShot = true;
            }
        }
    }

    [PunRPC]
    protected void ApplyDamage(int targetId)
    {
        GameObject target = Managers.game.RemoteTargetFinder(targetId);

        //플레이어 
        if (target.CompareTag("PLAYER"))
        {
            //초기화
            PlayerStats target_pStats = target.GetComponent<PlayerStats>();

            //데미지 피해
            target_pStats.receviedDamage = (_pv.ViewID, _pStats.basicAttackPower);

            //타겟 사망 시
            if (target_pStats.nowHealth <= 0)
            {
                BaseCard._lockTarget = null;
            }
        }
        //오브젝트
        if (!target.CompareTag("PLAYER"))
        {
            //초기화
            ObjStats target_oStats = target.GetComponent<ObjStats>();

            //데미지 피해
            target_oStats.nowHealth -= _pStats.basicAttackPower;

            //타겟 사망 시
            if (target_oStats.nowHealth <= 0)
            {
                BaseCard._lockTarget = null;
            }
        }
    }
}
