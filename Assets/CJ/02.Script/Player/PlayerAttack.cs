using UnityEngine;

public partial class PlayerManager
{
    public GameObject Bullet;
    public Transform BulletPos;
    public float BulletSpeed;
    void Attack()
    {
        Instantiate(Bullet, BulletPos.position, BulletPos.rotation);
    }
}
