using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private Vector3 shootDirection = Vector3.right;

    [Tooltip("Delay (in seconds) between shots.")]
    [SerializeField]
    private float fireRate = 0.5f;

    private float nextFireTime;

    void Start()
    {
        // Initialize nextFireTime so the player can shoot immediately
        nextFireTime = 0f;
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned in the Shoot script!", this);
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire Point is not assigned in the Shoot script!", this);
            return;
        }

        GameObject newBulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet newBullet = newBulletGO.GetComponent<Bullet>();

        if (newBullet != null)
        {
            newBullet.movementDirection = shootDirection;
        }
    }
}