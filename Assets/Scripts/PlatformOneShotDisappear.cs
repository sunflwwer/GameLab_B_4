using UnityEngine;

public class PlatformOneShotDisappear : MonoBehaviour
{
    [SerializeField] float disappearDelay = 0.6f;   // 밟고 나서 사라지기까지 시간
    [SerializeField] float destroyDelay = 0.1f;     // 콜라이더 끈 뒤 완전 삭제까지 시간
    [SerializeField] string playerTag = "Player";   // 플레이어 태그

    bool triggered;
    Collider col;
    Renderer[] renderers;

    void Awake()
    {
        col = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!triggered && other.gameObject.CompareTag(playerTag))
        {
            triggered = true;
            Invoke(nameof(DisappearNow), disappearDelay);
        }
    }

    // 만약 발판 콜라이더를 Trigger로 쓸 경우 이 메서드도 사용 가능
    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject.CompareTag(playerTag))
        {
            triggered = true;
            Invoke(nameof(DisappearNow), disappearDelay);
        }
    }

    void DisappearNow()
    {
        // 더 이상 밟을 수 없도록 콜라이더 비활성
        if (col) col.enabled = false;

        // 시각적으로도 바로 사라지게 렌더러 비활성
        foreach (var r in renderers) r.enabled = false;

        // 약간의 여유 뒤 완전 삭제
        Destroy(gameObject, destroyDelay);
    }
}
