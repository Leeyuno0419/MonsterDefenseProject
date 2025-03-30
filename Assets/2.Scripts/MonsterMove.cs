using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;   // 왼쪽 이동 속도

    [Header("Jump Settings")]
    public float jumpForce = 2f;         // 점프 힘
    public float checkDistance = 0.2f;   // 앞 몬스터 감지용 Ray 거리

    [Header("Layer Settings")]
    public LayerMask monsterLayer;       // 몬스터 레이어

    [Header("Jump Cooldown")]
    public float jumpCooldown = 0.2f;    // 점프 쿨타임
    private float jumpTimer = 0f;  

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.fixedDeltaTime;
        }

        Rigidbody2D frontRb = GetFrontMonsterRb();
        bool monsterOnTop = IsMonsterOnTop();

        // 점프 조건: 앞에 몬스터 있고, 위에 몬스터 없고, 쿨타임 끝나고, 앞 몬스터가 점프 중이 아닐 때
        bool canJump = frontRb != null && frontRb.velocity.y <= 0.1f;

        if (frontRb != null && !monsterOnTop && jumpTimer <= 0f && canJump)
        {
            Jump();
            jumpTimer = jumpCooldown;
        }

        // 이동 방향 처리
        float moveDirection = -moveSpeed; //기본 이동 방향 -x축

        if (monsterOnTop)
        {
            moveDirection = moveSpeed * 0.1f;
        }

        rb.velocity = new Vector2(moveDirection, rb.velocity.y);
    }

    Rigidbody2D GetFrontMonsterRb() // 앞에 있는 몬스터 감지
    {
        Vector2 rayOrigin = new Vector2(transform.position.x - 0.6f, transform.position.y + 0.3f);
        Debug.DrawRay(rayOrigin, Vector2.left * checkDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, checkDistance, monsterLayer);

        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return hit.collider.attachedRigidbody;
        }

        return null;
    }

    bool IsMonsterOnTop() // 위에 있는 몬스터 감지
    {
        Vector2 boxOrigin = new Vector2(transform.position.x - 0.4f, transform.position.y + 1.2f);
        Vector2 boxSize = new Vector2(0.3f, 0.2f);

        Collider2D hit = Physics2D.OverlapBox(boxOrigin, boxSize, 0f, monsterLayer);
        return hit != null && hit.gameObject != gameObject;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void SetLineLayer(string layerName)  // 레이어 설정
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            gameObject.layer = layer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = layer;
            }
        }
        else
        {
            Debug.LogWarning($"잘못된 레이어 이름: {layerName}");
        }
    }
}
