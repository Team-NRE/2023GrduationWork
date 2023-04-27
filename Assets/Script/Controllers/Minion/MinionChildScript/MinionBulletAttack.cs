using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class MinionBulletAttack : MonoBehaviour
{
    [Header ("- Bullet Info")]
    [SerializeField] GameObject target;
    [SerializeField] float damage;
    [SerializeField] float speed;

    public void setBulletInfo(GameObject _Target, float _Damage)
    {
        target = _Target;
        damage = _Damage;
    }

    void FixedUpdate()
    {
        if (target == null) 
            Destroy(this.gameObject);
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
            
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                target.GetComponent<ObjStats>().NowHealth -= damage;
                Destroy(this.gameObject);
            }
        }
    }
}
