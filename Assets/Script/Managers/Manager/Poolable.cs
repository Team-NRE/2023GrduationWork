using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool IsUsing;    //???? ??? ?????? ??????.

    public virtual void Proj_Target_Init(GameObject _target) { }

    public virtual void Proj_Target_Init(Vector3 _shooter, Transform _target, float bulletSpeed, float damage) { }
}
