using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    Vector3 targetPos;

    float Bullet_y;
    float _shotPower;

    public void target_set(GameObject target, float shotPower)
    {
        _target = target;
        _shotPower = shotPower;

    }

    void Update()
    {
        if (_target == null) { Destroy(this.gameObject); }

        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _shotPower);
            targetPos = _target.transform.position;

            if (Vector3.Distance(transform.position, _target.transform.position) <= 0.5f)
            {
                _target.GetComponent<Stats>().NowHealth -= PlayerManager.Player_Instance.player_stats.attackPower;
                Destroy(this.gameObject);
            }
        }
    }
}
