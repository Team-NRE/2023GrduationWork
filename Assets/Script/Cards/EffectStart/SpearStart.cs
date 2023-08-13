using UnityEngine;
using Stat;

public class SpearStart : MonoBehaviour
{
    float bulletSpeed;
    float damage = default;
    int enemylayer = default;

    Transform playerTr;
    PlayerStats pStats;

    public void StartSpear(string player, int _enemylayer, float _damage)
    {
        GameObject _player = GameObject.Find(player);

        pStats = _player.GetComponent<PlayerStats>();
        playerTr = _player.transform;

        enemylayer = _enemylayer;
        bulletSpeed = 30f; // 공속 대비 5배 속도
        damage = _damage;
    }

    public void Update()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        Vector3 SpearDirection = playerTr.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();

                enemyStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }

            Destroy(this.gameObject);
        }
    }
}
