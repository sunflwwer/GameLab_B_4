using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
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
        Destroy(gameObject); // 플레이어 오브젝트 파괴
        GameManager.Instance.SpawnPlayer(); // 리스폰 호출
    }
}
