using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : BaseController
{
	//DefaultSetting is Targeting skill
	Define.Projectile _projType = Define.Projectile.Undefine;

	//Get Dir from Character Quaternion
	public abstract Transform GetPlayerTransform();

	//Get Target's Info
	public abstract GameObject GetTargetTransform();


}
