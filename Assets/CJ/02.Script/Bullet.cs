using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject _target;
    float _shotPower;

    public void target_set(GameObject target, float shotPower)
    {
        _target = target;
        _shotPower = shotPower;

    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _shotPower);

        if (Vector3.Distance(transform.position, _target.transform.position) <= 0.6f)
        {
            Destroy(this.gameObject);
        }
    }
}
