using UnityEngine;

public class PlatformOneShotDisappear : MonoBehaviour
{
    [SerializeField] float disappearDelay = 0.6f;   // ��� ���� ���������� �ð�
    [SerializeField] float destroyDelay = 0.1f;     // �ݶ��̴� �� �� ���� �������� �ð�
    [SerializeField] string playerTag = "Player";   // �÷��̾� �±�

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

    // ���� ���� �ݶ��̴��� Trigger�� �� ��� �� �޼��嵵 ��� ����
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
        // �� �̻� ���� �� ������ �ݶ��̴� ��Ȱ��
        if (col) col.enabled = false;

        // �ð������ε� �ٷ� ������� ������ ��Ȱ��
        foreach (var r in renderers) r.enabled = false;

        // �ణ�� ���� �� ���� ����
        Destroy(gameObject, destroyDelay);
    }
}
