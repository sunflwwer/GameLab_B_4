using UnityEngine;

public class ToggleBox : MonoBehaviour
{
    [SerializeField] float interval = 3f; // �� �ʸ��� �Ѱ� ����
    private bool isActive = true;

    void Start()
    {
        // 3�ʸ��� ToggleActive �Լ��� �ݺ� ����
        InvokeRepeating(nameof(ToggleActive), interval, interval);
    }

    void ToggleActive()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
