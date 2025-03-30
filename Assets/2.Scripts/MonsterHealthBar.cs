using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("ü�¹� �̹���")]
    public Image frontFill;  // ��� �����ϴ� ������ ��
    public Image backFill;   // õõ�� ������� �Ͼ�� ��

    [Header("�ӵ� ����")]
    public float frontSpeed = 10f;  // ������ �ӵ�
    public float backSpeed = 2f;    // �Ͼ�� �ӵ�

    private float targetFill = 1f;

    void Update()
    {
        if (frontFill != null)
        {
            frontFill.fillAmount = Mathf.MoveTowards(frontFill.fillAmount, targetFill, Time.deltaTime * frontSpeed);
        }

        if (backFill != null)
        {
            backFill.fillAmount = Mathf.MoveTowards(backFill.fillAmount, targetFill, Time.deltaTime * backSpeed);
        }
    }

    public void SetHealth(float normalized)
    {
        targetFill = Mathf.Clamp01(normalized); // ü�� ���� (0 ~ 1)
    }
}
