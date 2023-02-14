using UnityEngine;

public partial class PlayerManager
{
    public GameObject AttackRangeimg;


    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    //public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    //[SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    //[SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;


    //사거리 표시 껏다 켜기
    public GameObject GetAttackRange(string AttackRange)
    {
        GameObject check = null;
        check = GameObject.Find(AttackRange);
        
        if (checkAttack == false || checkAttack == true)
        {
            checkAttack = !checkAttack;
            AttackRangeimg.SetActive(checkAttack);
        }

        return check;
    }

    void Attack()
    {
        GetAttackRange("AttackRange");

        //a키 누르기 -> 사거리 켜기 -> 원 둘레 안 적 식별 -> 적이면 애니메이션 및 공격 작용.
        // 완       ->     완      ->  physics. 
    }

    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bulletPrefab.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

}
