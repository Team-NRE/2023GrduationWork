using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStats : MonoBehaviour
{
    public int _cost;
    public float _damage;
    public float _defence;
    public float _debuff;
    public float _buff;
    public GameObject _range;

    public int cost { get { return _cost; } set { value = _cost; } }
    public float damage { get { return _damage; } set { value = _damage; } }
    public float defence { get { return _defence; } set { value = _defence; } }
    public float debuff { get { return _debuff; } set { value = _debuff; } }
    public float buff { get { return _buff; } set { value = _buff; } }
    public GameObject range { get { return _range; } set { value = _range; } }

}
