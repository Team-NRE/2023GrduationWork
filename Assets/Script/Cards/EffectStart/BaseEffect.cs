using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AddComponent �Ϲ�ȭ�� ���� Ŭ����
public class BaseEffect : MonoBehaviour
{
	protected PhotonView _pv;

	protected IEnumerator DelayDestroy(GameObject target, float time)
	{
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(target);
	}

	// �����ε� �Լ�
	protected void StartSpec(int id, float effectTime, float saveMaxHealth, float saveNowHealth)
	{

	}

	protected void StartSpec()
	{

	}
}
