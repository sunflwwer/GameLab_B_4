using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] float bounceForce = 20f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Platform ø° ¥Í¿∏∏È ∆®∞‹ø¿∏£±‚
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }
}
