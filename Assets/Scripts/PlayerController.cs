using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float cameraRotationSpeed = 0.05f;


    public Camera playerCamera;


    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    float initSpeedTime = 1.0f;

    bool item = true; // temp


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    void OnLook(InputValue value)
    {

        lookInput = value.Get<Vector2>();
    }

    void ProcessLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed);

        Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
        cameraRotation.x -= lookInput.y * cameraRotationSpeed;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, 10f, 20f);
        playerCamera.transform.localEulerAngles = cameraRotation;
    }


    void FixedUpdate()
    {
        ProcessMove();
    }

    private void LateUpdate()
    {
        ProcessLook();
    }


    Vector3 GetMoveDirection()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        camForward.y = 0; // 수평 방향만 사용
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();



        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;
        return moveDir;
    }

    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }

    void OnJump()
    {
        // 현재 가속도 초기화 후 impulse 힘 적용
        // 아이템이 있을때에만 점프 가능
        if (item)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            item = false;
        }
    }

    void OnDash()
    {
        if (item)
        {
            StartCoroutine(DashCoroutine());
            item = false;
        }
    }

    IEnumerator DashCoroutine()
    {
        Vector3 dashDir = GetMoveDirection();
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
}
