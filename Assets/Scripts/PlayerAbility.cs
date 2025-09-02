using UnityEngine;

using System.Collections;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] PlayerController PlayerController;
    [SerializeField] float jumpForce = 20.0f;

    Rigidbody rb;

    bool hasDoubleJump = false;
    bool hasDash = false;

    float initSpeedTime = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnJump()
    {
        // 현재 가속도 초기화 후 impulse 힘 적용
        // 아이템이 있을때에만 점프 가능
        if (hasDoubleJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasDoubleJump = false;
        }
    }

    void OnDash()
    {
        if (hasDash)
        {
            StartCoroutine(DashCoroutine());
            hasDash = false;
        }
    }

    IEnumerator DashCoroutine()
    {
        Vector3 dashDir = PlayerController.GetMoveDirection();
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }

        // 대시 속도 직접 설정
        rb.linearVelocity = dashDir * jumpForce;

        // 짧은 시간 후 수평 속도 초기화
        yield return new WaitForSeconds(initSpeedTime);

        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    public void GiveAbility(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.DoubleJump:
                hasDoubleJump = true;
                break;
            case ItemType.Dash:
                hasDash = true;
                break;
            default:
                break;
        }
    }
}
