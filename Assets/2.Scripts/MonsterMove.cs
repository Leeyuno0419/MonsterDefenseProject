using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;   // ���� �̵� �ӵ�

    [Header("Jump Settings")]
    public float jumpForce = 2f;         // ���� ��
    public float checkDistance = 0.2f;   // �� ���� ������ Ray �Ÿ�

    [Header("Layer Settings")]
    public LayerMask monsterLayer;       // ���� ���̾�

    [Header("Jump Cooldown")]
    public float jumpCooldown = 0.2f;    // ���� ��Ÿ��
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

        // ���� ����: �տ� ���� �ְ�, ���� ���� ����, ��Ÿ�� ������, �� ���Ͱ� ���� ���� �ƴ� ��
        bool canJump = frontRb != null && frontRb.velocity.y <= 0.1f;

        if (frontRb != null && !monsterOnTop && jumpTimer <= 0f && canJump)
        {
            Jump();
            jumpTimer = jumpCooldown;
        }

        // �̵� ���� ó��
        float moveDirection = -moveSpeed; //�⺻ �̵� ���� -x��

        if (monsterOnTop)
        {
            moveDirection = moveSpeed * 0.1f;
        }

        rb.velocity = new Vector2(moveDirection, rb.velocity.y);
    }

    Rigidbody2D GetFrontMonsterRb() // �տ� �ִ� ���� ����
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

    bool IsMonsterOnTop() // ���� �ִ� ���� ����
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

    public void SetLineLayer(string layerName)  // ���̾� ����
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
            Debug.LogWarning($"�߸��� ���̾� �̸�: {layerName}");
        }
    }
}
