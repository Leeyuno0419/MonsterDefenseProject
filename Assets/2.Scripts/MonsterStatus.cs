using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHP = 100;
    private int currentHP;

    public bool IsDead => currentHP <= 0;

    [Header("체력바 연동")]
    public MonsterHealthBar healthBar; // 체력바 스크립트

    [Header("데미지 텍스트 설정")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Transform damageTextSpawnPoint;

    void OnEnable()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log($"{gameObject.name}이(가) {amount}의 피해를 입음 (남은 HP: {currentHP})");

        ShowDamageText(amount);
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 사망했습니다");
        gameObject.SetActive(false); // 풀링 시스템을 위한 비활성화
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float ratio = (float)currentHP / maxHP;
            healthBar.SetHealth(ratio);

            // 체력비율이 1이면 비활성화, 아니면 활성화
            healthBar.gameObject.SetActive(ratio < 1f);
        }
    }

    void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null && damageTextSpawnPoint != null)
        {
            // 상하좌우로 랜덤 위치 오프셋
            Vector3 offset = new Vector3(
                Random.Range(-0.3f, 0.3f), // X축 랜덤
                Random.Range(-0.1f, 0.1f), // Y축 랜덤
                0f
            );

            Vector3 spawnPos = damageTextSpawnPoint.position + offset;

            GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
            DamageText dmg = obj.GetComponent<DamageText>();
            if (dmg != null)
            {
                dmg.SetText(damage);
            }
        }
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
}
