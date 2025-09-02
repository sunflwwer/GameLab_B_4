using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform targetPosition; // 이동시킬 위치 (빈 오브젝트를 만들어 할당)

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player 태그를 가진 오브젝트만
        {
            other.transform.position = targetPosition.position; // 특정 위치로 이동
        }
    }
}