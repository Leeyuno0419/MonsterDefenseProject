using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float lifeTime = 3f;

    public void Setup(int damage)
    {
        this.damage = damage;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterStatus status = other.GetComponent<MonsterStatus>();
            if (status != null)
            {
                status.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
