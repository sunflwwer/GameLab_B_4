using UnityEngine;

public class Player : MonoBehaviour
{
    public float fallThreshold = -10f; // 이 값보다 y좌표가 작아지면 낙사 처리

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 낙사 체크
        if (transform.position.y < fallThreshold)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.SpawnPlayer(); // 리스폰 호출
    }
}
