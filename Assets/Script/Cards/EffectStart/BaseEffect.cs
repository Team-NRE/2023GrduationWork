using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AddComponent �Ϲ�ȭ�� ���� Ŭ����
public class BaseEffect : MonoBehaviour
{
	//Stat
    ///Stat Value
    public float damageValue { get; set; }
	public (float,float) powerValue { get; set; }
	
	public float speedValue { get; set; }
	public float attackSpeedValue { get; set; }
	public float projectileSpeedValue { get; set; }
    
	public float shieldValue { get; set; }
	public float healthRegenValue { get; set; }
	public float manaRegenValue { get; set; }

    ///Stat bool
	public bool invincibleTime { get; set; }


	//초기화
    ///이펙트 관련 초기화
    public PhotonView effectPV {  get; set; }
    public Quaternion effectRot { get; set; }
    public float effectTime { get; set; }
    public float startEffect { get; set; }

    ///player & playerTeam 관련 초기화
    public GameObject player { get; set; }
    public PlayerStats pStat { get; set; }
    public PhotonView playerPV { get; set; }
    public int playerId { get; set; }
    public int teamLayer { get; set; }

    ///target & targetTeam 관련 초기화
    public GameObject target { get; set; }
    public PlayerStats target_pStat { get; set; }
    public ObjStats target_oStat { get; set; }
    public PhotonView targetPV { get; set; }
    public int targetId { get; set; }
    public int enemyLayer { get; set; }


    public virtual void CardEffectInit(int userId)
	{
		player = GetRemotePlayer(userId);
		pStat = player.GetComponent<PlayerStats>();
		playerPV = player.GetComponent<PhotonView>();
		playerId = userId;
	}

	public virtual void CardEffectInit(int userId, int remoteTargetId)
	{
		player = GetRemotePlayer(userId);
		pStat = player.GetComponent<PlayerStats>();
		playerPV = player.GetComponent<PhotonView>();
		playerId = userId;

		target = GetRemotePlayer(remoteTargetId);
        targetPV = target.GetComponent<PhotonView>();
        targetId = remoteTargetId;
	}
	public virtual void CardEffectInit(int userId, Quaternion effectRotation)
	{
        player = GetRemotePlayer(userId);
        pStat = player.GetComponent<PlayerStats>();
        playerPV = player.GetComponent<PhotonView>();
        playerId = userId;

        effectRot = effectRotation;
	}

	public virtual IEnumerator CardEffectInit(int userId, float time)
    {
		player = GetRemotePlayer(userId);
		yield return new WaitForSeconds(0);
    }

	protected IEnumerator DelayTimer(float time)
	{
		yield return new WaitForSeconds(time);
	}

	protected GameObject GetRemotePlayer(int remoteId)
	{
		GameObject target = PhotonView.Find(remoteId)?.gameObject;
		return target;
	}

	[PunRPC]
	protected void RpcDelayDestroy(int id, float time)
    {
        GameObject target = GetRemotePlayer(id);
		StartCoroutine(DelayTimer(time));
		PhotonNetwork.Destroy(target);
    }

	[PunRPC]
	protected void GetRemoteParent(int id)
    {
		GameObject parent = GetRemotePlayer(id);
		parent.transform.parent = parent.transform;
    }

	public Quaternion GetDirectionalVector(Vector3 ground, Transform playerTr)
	{
		Quaternion rotation = new Quaternion();
		Vector3 targetDir = playerTr.position - ground;
		rotation = Quaternion.Euler(targetDir);

		return rotation;
	}
}
