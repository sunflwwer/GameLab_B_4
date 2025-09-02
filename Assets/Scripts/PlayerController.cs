using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float cameraRotationSpeed = 0.05f;


    public Camera playerCamera;


    Rigidbody rb;
    Vector2 moveInput;
    Vector2 lookInput;
    

    


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

    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }

    
}
