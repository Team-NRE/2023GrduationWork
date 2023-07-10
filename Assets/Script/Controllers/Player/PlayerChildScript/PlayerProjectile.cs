using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 타게팅 객체를 상정하고 만든다.
public class PlayerProjectile : BaseProjectile
{
	public GameObject pTarget;
	public GameObject pAttacker;
	private PhotonView _pv;

	//protected GameObject _target;
	//protected GameObject _attacker;
	//protected float _damage;
	public Vector3 targetVec;

	//public Define.Projectile ProjType { get; set; } = Define.Projectile.Undefine;

	public void Awake()
	{
		_pv = GetComponent<PhotonView>();
		ProjType = Define.Projectile.Attack_Proj;
	}

	public override void Init(GameObject target, GameObject attacker)
	{
		pTarget = target;
		pAttacker = attacker;
		//ProjType = Define.Projectile.Attack_Proj;
		_damage = 5.0f;
	}

	public void Update()
	{
		if (_pv.IsMine)
		{
			if (pTarget != null || pAttacker != null)
			{
				Fire(pTarget, pAttacker);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			if (pTarget != null || pAttacker != null)
			{
				_pv.RPC("Fire", RpcTarget.All, pTarget, pAttacker);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}
	}

	[PunRPC]
	public override void Fire(GameObject target, GameObject attacker)
	{
		targetVec = new Vector3(pTarget.transform.position.x, transform.position.y, pTarget.transform.position.z);
		//Vector3 targetVec = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
		this.transform.position = Vector3.Lerp(this.transform.position, targetVec, Time.deltaTime * 5.0f);
		Debug.Log($"Start : {this.gameObject.transform}, Dest : {pTarget.gameObject.transform}");
		
		//일단 평타는 무조건 타게팅이란 전제
		if (Vector3.Distance(transform.position, targetVec) <= 0.7f)
		{
			if (this.ProjType == Define.Projectile.Attack_Proj)
			{
				if (pTarget.gameObject.tag != "PLAYER")
				{
					//_pv.RPC("NetObjectDamage", RpcTarget.All, pTarget);
					NetObjectDamage(pTarget);
				}
				else
				{
					//_pv.RPC("NetPlayerDamage", RpcTarget.All, pTarget);
					NetPlayerDamage(pTarget);
				}
			}
			else
			{
				Debug.Log($"{this.gameObject.name} Type is not firmed");
			}
			//if(PhotonNetwork.IsMasterClient)
			//	PhotonNetwork.Destroy(this.gameObject);
			Destroy(this.gameObject, 2.0f);
			Destroy(this.gameObject);
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
}
