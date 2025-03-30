using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float duration = 0.5f;

    private TextMeshPro textMesh;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void SetText(int damage)
    {
        textMesh.text = damage.ToString();
        timer = duration;
    }

    void Update()
    {
        // ���� ��������
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // �������
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
