using UnityEngine;

/// <summary>
/// 바운스(튕김) 효과를 주는 컴포넌트
/// </summary>
public class Bouncy : MonoBehaviour
{
    [SerializeField] PlayerEffect playerEffect; // 이펙트 처리용 컴포넌트
    [SerializeField] float bounceForce = 20f;   // 튕기는 힘

    Rigidbody rb; // 플레이어 Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 캐싱
    }

    // 플랫폼과 충돌 시 튕김 효과 적용
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse); // 위로 힘을 가함
            playerEffect.TriggerParticle(EffectType.Jump);            // 점프 이펙트 실행
        }
    }
}
