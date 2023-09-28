using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public class Police : Players
{
    #region Variable
    //총알 위치
    private Transform _Proj_Parent;
    public GameObject target;
    private GameObject _netBullet;
    #endregion


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
        _pType = Define.PlayerType.Police;
        if (_pv.IsMine)
            _pv.RPC(
                "PlayerStatSetting",
                RpcTarget.All,
                _pType.ToString(),
                PhotonNetwork.LocalPlayer.NickName
            );

        //총알 위치
        _Proj_Parent = this.transform.Find("Location");
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
            if (_pv.IsMine)
            {
                //Shoot
                string tempName = "PoliceBullet";
                _netBullet = PhotonNetwork.Instantiate(tempName, _Proj_Parent.position, _Proj_Parent.rotation);
                PhotonView localPv = _netBullet.GetComponent<PhotonView>();
                localPv.RPC("Init", RpcTarget.All, _pv.ViewID, BaseCard._lockTarget.GetComponent<PhotonView>().ViewID);
            }
        }
        //평타 쿨타임
        _stopAttack = true;

        return;
    }
}
