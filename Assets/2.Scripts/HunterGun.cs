using UnityEngine;

public class HunterGun : MonoBehaviour
{
    [Header("��� ����")]
    public GameObject bulletPrefab;
    public float shootInterval = 1.5f;
    public float bulletSpeed = 15f;
    public int bulletDamage = 30;
    public int bulletCount = 5;
    public float spreadAngle = 12f;

    [Header("���� ����")]
    public float detectionRadius = 10f;

    [Header("�� ���� �� �߻� ��ġ")]
    [SerializeField] private Transform gunSprite;       // �� ��������Ʈ�� �Ҵ�
    [SerializeField] private Transform bulletSpawn;     // �Ѿ� �߻� ��ġ

    private float shootTimer = 0f;
    private Transform target;
    private float currentAimAngle = 0f;

    void Update()
    {
        shootTimer += Time.deltaTime;

        target = FindClosestMonster();
        if (target != null)
        {
            AimAtTarget(target);

            if (shootTimer >= shootInterval)
            {
                ShootSpread();
                shootTimer = 0f;
            }
        }
    }

    Transform FindClosestMonster()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Monster"))
            {
                float dist = Vector2.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = col.transform;
                }
            }
        }

        return closest;
    }

    void AimAtTarget(Transform target)
    {
        Vector2 dir = target.position - transform.position;
        currentAimAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // �� ��������Ʈ�� �ð��� ������
        if (gunSprite != null)
            gunSprite.rotation = Quaternion.Euler(0f, 0f, currentAimAngle - 35f);
    }

    void ShootSpread()
    {
        float halfSpread = spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float randomOffset = Random.Range(-halfSpread, halfSpread);
            float shotAngle = currentAimAngle + randomOffset;

            Quaternion rotation = Quaternion.Euler(0f, 0f, shotAngle - 90f);
            Vector2 direction = Quaternion.Euler(0f, 0f, shotAngle) * Vector2.right;

            // �Ѿ� �߻� ��ġ���� ����
            Vector3 spawnPosition = bulletSpawn != null ? bulletSpawn.position : transform.position;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = direction * bulletSpeed;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
                bulletScript.Setup(damage: bulletDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
