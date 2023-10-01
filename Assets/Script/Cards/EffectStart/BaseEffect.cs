using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AddComponent �Ϲ�ȭ�� ���� Ŭ����
public class BaseEffect : MonoBehaviour
{
	//이펙트 발동 시간
    public float _effectTime;

	//Stat 
    public float _damage;
    public float _defence;
    public float _speed;
    public float _buff;
    public float _debuff;
    //부활
    public bool _IsResurrection;

	public GameObject player = null;
	public GameObject target = null;

	public virtual void CardEffectInit(int userId)
	{
		player = GetRemotePlayer(userId);
		//Debug.Log("1 param");
	}

	public virtual void CardEffectInit(int userId, int targetId)
	{
		player = GetRemotePlayer(userId);
		target = GetRemotePlayer(targetId);
		//Debug.Log("2 param");
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
