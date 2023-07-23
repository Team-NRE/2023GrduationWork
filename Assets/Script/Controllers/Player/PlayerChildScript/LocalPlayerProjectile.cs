using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 1. 포톤에선 GameObject를 매개변수로 넘겨줄 수 없다.
/// 2. 시스템을 이원화 한다
///		- Vector로 추적하는 부분 따로
///		- 데미지를 적용하는 부분을 따로
///	3. 탄 인스턴스는 로컬로 하고 네트워크 인스턴스를 하지 않는다.
/// </summary>

// 타게팅 객체를 상정하고 만든다.
public class LocalPlayerProjectile : MonoBehaviour
{
	public GameObject pTarget;
	//public GameObject pAttacker;
	public string targetName;
	public Vector3 targetVector;
	public Vector3 attackVector;
	private PhotonView _pv;
	private float _damage;
	protected float damping = 10.0f;

	//public Vector3 targetVec;

	public Define.Projectile ProjType { get; set; } = Define.Projectile.Undefine;

	public void Awake()
	{
		_pv = GetComponent<PhotonView>();
		ProjType = Define.Projectile.Attack_Proj;
		_damage = 5.0f;
	}

	public void Init(string name, Vector3 targetVec)
	{
		targetName = name;
		targetVector = targetVec;
	}

	public GameObject SetRemoteTarget(string target)
	{
		string targetName = target;
		GameObject returnTarget = GameObject.Find(targetName);
		return pTarget = returnTarget;
	}

	void Update()
	{
		_pv.RPC("Fire", RpcTarget.All, targetName, targetVector);
	}

	[PunRPC]
	public void Fire(string target, Vector3 targetVec)
	{
		pTarget = SetRemoteTarget(target);
		Debug.Log($"{target}, {pTarget}, {targetVector}");	// 이게 왜 null인지 모르겠네요
		this.transform.position = Vector3.Lerp(this.transform.position, targetVec, Time.deltaTime * 5.0f);
		//일단 평타는 무조건 타게팅이란 전제
		if (Vector3.Distance(transform.position, targetVec) <= 0.7f)
		{
			if (this.ProjType == Define.Projectile.Attack_Proj)
			{
				Damage();
			}
			else
			{
				Debug.Log($"{this.gameObject.name} Type is not firmed");
			}
			//PhotonNetwork.Destroy(this.gameObject, 2.0f);
			PhotonNetwork.Destroy(this.gameObject);
		}
	}


	public void NetPlayerDamage(GameObject target)
	{
		Stat.PlayerStats _Stats = target.GetComponent<Stat.PlayerStats>();
		_Stats.nowHealth -= _damage;

		Debug.Log($"{_Stats.nowHealth}");
	}
	
	public void NetObjectDamage(GameObject target)
	{
		Stat.ObjStats _Stats = target.GetComponent<Stat.ObjStats>();
		_Stats.nowHealth -= _damage;

		Debug.Log($"{_Stats.nowHealth}");
	}

	public void Damage()
	{
		if (pTarget.gameObject.tag != "PLAYER")
		{
			NetObjectDamage(pTarget);
		}
		else
		{
			NetPlayerDamage(pTarget);
		}
	}
}