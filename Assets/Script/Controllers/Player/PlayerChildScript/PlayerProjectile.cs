using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 타게팅 객체를 상정하고 만든다.
public class PlayerProjectile : BaseProjectile
{
	public GameObject pTarget;
	public GameObject pAttacker;

	public override void Init(GameObject target, GameObject attacker)
	{
		pTarget = target;
		pAttacker = attacker;
		ProjType = Define.Projectile.Attack_Proj;
		_damage = 5.0f;
	}

	public void Update()
	{
		Fire(pTarget, pAttacker);
	}

	public override void Fire(GameObject target, GameObject attacker)
	{
		Vector3 targetVec = new Vector3(pTarget.transform.position.x, transform.position.y, pTarget.transform.position.z);
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
					Stat.ObjStats _Stats = pTarget.GetComponent<Stat.ObjStats>();
					_Stats.nowHealth -= _damage;
				}
				else
				{
					Stat.PlayerStats _Stats = pTarget.GetComponent<Stat.PlayerStats>();
					_Stats.nowHealth -= _damage;
				}
			}
			else
			{
				Debug.Log($"{this.gameObject.name} Type is not firmed");
			}
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}
