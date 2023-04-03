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
    public GameObject GetAttackRange()
    {
        if (checkAttack == false || checkAttack == true)
        {
            checkAttack = !checkAttack;
            AttackRangeimg.SetActive(checkAttack);
        }

        return AttackRangeimg;
    }

    void Attack()
    {
        GetAttackRange();
        //a키 누르기 -> 사거리 켜기 -> 원 둘레 안 적 식별 -> 적이면 애니메이션 및 공격 작용.
        // 완       ->     완      ->  physics. 
    }

    void Attack_Detection(Vector3 pos, float radius)
    {
        Collider[] colls = Physics.OverlapSphere(pos, radius, layerMask);

        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].SendMessage("Check", SendMessageOptions.DontRequireReceiver);
        }

    }

    public void Shoot()
    {
        state = State.Attack;
        
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

        GetAttackRange();
    }

}
