using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 이동 및 카메라 회전 컨트롤러
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;                // 이동 속도
    [SerializeField] float rotationSpeed = 0.1f;           // 캐릭터 회전 속도
    [SerializeField] float cameraRotationSpeed = 0.05f;    // 카메라 상하 회전 속도
    [SerializeField] float minPitch = -30f;                // 카메라 최소 각도
    [SerializeField] float maxPitch = 60f;                 // 카메라 최대 각도
    [SerializeField] float cameraDistance = 5f;            // 카메라와 캐릭터 거리


    public Camera playerCamera;                            // 플레이어를 따라다니는 카메라


    Rigidbody rb;                                          // 플레이어 Rigidbody
    Vector2 moveInput;                                     // 이동 입력값
    Vector2 lookInput;                                     // 마우스 입력값
    float cameraPitch = 0f;                                // 카메라 상하 각도
    


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

    /// <summary>
    /// 마우스 입력으로 캐릭터와 카메라 회전 처리
    /// </summary>
    void ProcessLook()
    {
        // 마우스 좌우 입력으로 캐릭터 회전
        float yaw = transform.eulerAngles.y + lookInput.x * rotationSpeed;
        // 마우스 상하 입력으로 카메라 pitch 변경
        cameraPitch -= lookInput.y * cameraRotationSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        // 캐릭터 Y축 회전 적용
        transform.eulerAngles = new Vector3(0f, yaw, 0f);

        // 카메라 위치 및 회전 계산 (캐릭터를 중심으로 회전)
        Vector3 offset = Quaternion.Euler(cameraPitch, yaw, 0f) * new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f); // 캐릭터 머리 높이 바라봄
    }


    void FixedUpdate()
    {
        ProcessMove();
    }

    private void LateUpdate()
    {
        ProcessLook();
    }

    /// <summary>
    /// 카메라 기준으로 이동 방향 반환
    /// </summary>
    public Vector3 GetMoveDirection()
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

    /// <summary>
    /// 이동 처리
    /// </summary>
    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }    
}
