using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] PlayerEffect playerEffect;
    [SerializeField] float bounceForce = 20f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // 모든 충돌 지점을 검사
            foreach (ContactPoint contact in collision.contacts)
            {
                // 충돌 표면의 법선이 위쪽을 향할 때만 튀게 함
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
                {
                    rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                    playerEffect.TriggerParticle(EffectType.Jump);
                    break;
                }
            }
        }
    }
}
