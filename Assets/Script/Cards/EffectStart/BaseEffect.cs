using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AddComponent 일반화를 위한 클래스
public class BaseEffect : MonoBehaviour
{
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
}
