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

	//�ʱ�ȭ
	public virtual void Init(GameObject target, GameObject attacker) { }

	// ���⼭ ��ü�� ������ �Ǻ� �� �߻��Լ� ȣ��
	public virtual void Fire(GameObject target, GameObject attacker) { }
	// Ÿ���� ��ü�� ���� ó��
	public Vector3 GetTrack(Transform target, Transform attacker) 
	{
		Vector3 dirVec;
		Vector3 targetVec = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 attackerVec = new Vector3(attacker.position.x, attacker.position.y, attacker.position.z);

		return dirVec = targetVec - attackerVec;
	}
	// ��Ÿ�� ��ü�� ���� ó��
	public void GetShoot(Transform attacker, GameObject bullet) { }
}
