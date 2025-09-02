using UnityEngine;

public class ToggleBox : MonoBehaviour
{
    [SerializeField] float interval = 3f; // 몇 초마다 켜고 끌지
    private bool isActive = true;

    void Start()
    {
        // 3초마다 ToggleActive 함수를 반복 실행
        InvokeRepeating(nameof(ToggleActive), interval, interval);
    }

    void ToggleActive()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
