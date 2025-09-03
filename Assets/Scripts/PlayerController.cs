using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ?Œë ˆ?´ì–´ ?´ë™ ë°?ì¹´ë©”???Œì „ ì»¨íŠ¸ë¡¤ëŸ¬
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;                // ?´ë™ ?ë„
    [SerializeField] float rotationSpeed = 0.1f;           // ìºë¦­???Œì „ ?ë„
    [SerializeField] float cameraRotationSpeed = 0.05f;    // ì¹´ë©”???í•˜ ?Œì „ ?ë„
    [SerializeField] float minPitch = -30f;                // ì¹´ë©”??ìµœì†Œ ê°ë„
    [SerializeField] float maxPitch = 60f;                 // ì¹´ë©”??ìµœë? ê°ë„
    [SerializeField] float cameraDistance = 5f;            // ì¹´ë©”?¼ì? ìºë¦­??ê±°ë¦¬


<<<<<<< HEAD
    public Camera playerCamera;
    public Material glassMaterial; // Glass »ö»ó ¸ŞÅ×¸®¾ó
=======
    public Camera playerCamera;                            // ?Œë ˆ?´ì–´ë¥??°ë¼?¤ë‹ˆ??ì¹´ë©”??
>>>>>>> 11e7603b2f4c06818ff97962027b412ff03bd653


    Rigidbody rb;                                          // ?Œë ˆ?´ì–´ Rigidbody
    Vector2 moveInput;                                     // ?´ë™ ?…ë ¥ê°?
    Vector2 lookInput;                                     // ë§ˆìš°???…ë ¥ê°?
    float cameraPitch = 0f;                                // ì¹´ë©”???í•˜ ê°ë„
    


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
    /// ë§ˆìš°???…ë ¥?¼ë¡œ ìºë¦­?°ì? ì¹´ë©”???Œì „ ì²˜ë¦¬
    /// </summary>
    void ProcessLook()
    {
        // ë§ˆìš°??ì¢Œìš° ?…ë ¥?¼ë¡œ ìºë¦­???Œì „
        float yaw = transform.eulerAngles.y + lookInput.x * rotationSpeed;
        // ë§ˆìš°???í•˜ ?…ë ¥?¼ë¡œ ì¹´ë©”??pitch ë³€ê²?
        cameraPitch -= lookInput.y * cameraRotationSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        // ìºë¦­??Yì¶??Œì „ ?ìš©
        transform.eulerAngles = new Vector3(0f, yaw, 0f);

        // ì¹´ë©”???„ì¹˜ ë°??Œì „ ê³„ì‚° (ìºë¦­?°ë? ì¤‘ì‹¬?¼ë¡œ ?Œì „)
        Vector3 offset = Quaternion.Euler(cameraPitch, yaw, 0f) * new Vector3(0, 0, -cameraDistance);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.5f); // ìºë¦­??ë¨¸ë¦¬ ?’ì´ ë°”ë¼ë´?
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
    /// ì¹´ë©”??ê¸°ì??¼ë¡œ ?´ë™ ë°©í–¥ ë°˜í™˜
    /// </summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        camForward.y = 0; // ?˜í‰ ë°©í–¥ë§??¬ìš©
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();



        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;
        return moveDir;
    }

    /// <summary>
    /// ?´ë™ ì²˜ë¦¬
    /// </summary>
    private void ProcessMove()
    {
        Vector3 moveDir = GetMoveDirection();
        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);
    }

    // ÇÃ·¹ÀÌ¾î°¡ ¾î¶² ¿ÀºêÁ§Æ®¿Í Ãæµ¹ÇßÀ» ¶§
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangeToGlass"))
        {
            Renderer rend = collision.gameObject.GetComponent<Renderer>();
            if (rend != null && glassMaterial != null)
            {
                rend.material = glassMaterial;
            }
        }
    }
}
