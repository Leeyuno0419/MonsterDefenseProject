using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    [Header("ü�� ����")]
    public int maxHP = 100;
    private int currentHP;

    public bool IsDead => currentHP <= 0;

    [Header("ü�¹� ����")]
    public MonsterHealthBar healthBar; // ü�¹� ��ũ��Ʈ

    [Header("������ �ؽ�Ʈ ����")]
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

        Debug.Log($"{gameObject.name}��(��) {amount}�� ���ظ� ���� (���� HP: {currentHP})");

        ShowDamageText(amount);
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name}��(��) ����߽��ϴ�");
        gameObject.SetActive(false); // Ǯ�� �ý����� ���� ��Ȱ��ȭ
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float ratio = (float)currentHP / maxHP;
            healthBar.SetHealth(ratio);

            // ü�º����� 1�̸� ��Ȱ��ȭ, �ƴϸ� Ȱ��ȭ
            healthBar.gameObject.SetActive(ratio < 1f);
        }
    }

    void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null && damageTextSpawnPoint != null)
        {
            // �����¿�� ���� ��ġ ������
            Vector3 offset = new Vector3(
                Random.Range(-0.3f, 0.3f), // X�� ����
                Random.Range(-0.1f, 0.1f), // Y�� ����
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
