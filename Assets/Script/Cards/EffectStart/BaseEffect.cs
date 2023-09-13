using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AddComponent 일반화를 위한 클래스
public class BaseEffect : MonoBehaviour
{
	protected PhotonView _pv;

	protected IEnumerator DelayDestroy(GameObject target, float time)
	{
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(target);
	}

	// 오버로딩 함수
	protected void StartSpec(int id, float effectTime, float saveMaxHealth, float saveNowHealth)
	{

	}

	protected void StartSpec()
	{

	}
}
