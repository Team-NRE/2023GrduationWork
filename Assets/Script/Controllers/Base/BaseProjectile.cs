using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class BaseProjectile : MonoBehaviour
{
	//DefaultSetting is Targeting skill
	public Define.Projectile ProjType { get; set; } = Define.Projectile.Undefine;
	protected GameObject _target;
	protected GameObject _attacker;

	protected float _damage;
	protected Stat.PlayerStats _pStat;

	//초기화
	public virtual void Init(GameObject target, GameObject attacker) { }

	// 여기서 객체의 종류를 판별 후 발사함수 호출
	public virtual void Fire(GameObject target, GameObject attacker) { }
	// 타게팅 객체에 대한 처리
	public Vector3 GetTrack(Transform target, Transform attacker) 
	{
		Vector3 dirVec;
		Vector3 targetVec = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 attackerVec = new Vector3(attacker.position.x, attacker.position.y, attacker.position.z);

		return dirVec = targetVec - attackerVec;
	}
	// 논타겟 객체에 대한 처리
	public void GetShoot(Transform attacker, GameObject bullet) { }
}
