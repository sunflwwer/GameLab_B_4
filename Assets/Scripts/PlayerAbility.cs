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
        // ���� ���ӵ� �ʱ�ȭ �� impulse �� ����
        // �������� ���������� ���� ����
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

        // ��� �ӵ� ���� ����
        rb.linearVelocity = dashDir * jumpForce;

        // ª�� �ð� �� ���� �ӵ� �ʱ�ȭ
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
