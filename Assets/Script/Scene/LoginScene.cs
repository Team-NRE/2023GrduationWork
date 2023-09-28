using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
	
	private void Awake()
    {
		
    }

	private void Start()
	{
		GameObject pm = GameObject.Find("PhotonManager");
		if (pm == null)
			Managers.Resource.Instantiate("Prefabs/PhotonManager");
		Managers.UI.ShowSceneUI<UI_Login>();
	}

	public override void Clear()
	{
		
	}
}
