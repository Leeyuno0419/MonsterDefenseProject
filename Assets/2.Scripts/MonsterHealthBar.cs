using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("체력바 이미지")]
    public Image frontFill;  // 즉시 반응하는 빨간색 바
    public Image backFill;   // 천천히 따라오는 하얀색 바

    [Header("속도 설정")]
    public float frontSpeed = 10f;  // 빨간바 속도
    public float backSpeed = 2f;    // 하얀바 속도

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
        targetFill = Mathf.Clamp01(normalized); // 체력 비율 (0 ~ 1)
    }
}
