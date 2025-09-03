using UnityEngine;
using UnityEngine.InputSystem;

/// ?�레?�어 ?�동 �?카메???�전 컨트롤러
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;                // ?�동 ?�도
    [SerializeField] float rotationSpeed = 0.1f;           // 캐릭???�전 ?�도
    [SerializeField] float cameraRotationSpeed = 0.05f;    // 카메???�하 ?�전 ?�도
    [SerializeField] float minPitch = -30f;                // 카메??최소 각도
    [SerializeField] float maxPitch = 60f;                 // 카메??최�? 각도
    [SerializeField] float cameraDistance = 10f;            // 카메?��? 캐릭??거리

    public Camera playerCamera;
    //public Material glassMaterial; // Glass ���� ���׸���

    Rigidbody rb;                                          // ?�레?�어 Rigidbody
    Vector2 moveInput;                                     // ?�동 ?�력�?
    Vector2 lookInput;                                     // 마우???�력�?
    float cameraPitch = 0f;                                // 카메???�하 각도



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
    /// 마우???�력?�로 캐릭?��? 카메???�전 처리
    /// </summary>
    void ProcessLook()
    {
        // 마우??좌우 ?�력?�로 캐릭???�전
        float yaw = transform.eulerAngles.y + lookInput.x * rotationSpeed;
        // 마우???�하 ?�력?�로 카메??pitch 변�?
        cameraPitch -= lookInput.y * cameraRotationSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        // 캐릭??Y�??�전 ?�용
        transform.eulerAngles = new Vector3(0f, yaw, 0f);

        // 카메???�치 �??�전 계산 (캐릭?��? 중심?�로 ?�전)
        Vector3 offset = Quaternion.Euler(cameraPitch, yaw, 0f) * new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f); // 캐릭??머리 ?�이 바라�?
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

    /// 카메??기�??�로 ?�동 방향 반환
    /// 카메??기�??�로 ?�동 방향 반환

    /// </summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;


        camForward.y = 0; // ?�평 방향�??�용
        camForward.y = 0; // ?�평 방향�??�용
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();



        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;
        return moveDir;
    }

    /// <summary>
    /// ?�동 처리
    /// ?�동 처리
    /// </summary>
    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }

    // �÷��̾ � ������Ʈ�� �浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangeToGlass"))
        {
            Destroy(gameObject);
            GameManager.Instance.SpawnPlayer(); // ������ ȣ��
        }
    }
}
