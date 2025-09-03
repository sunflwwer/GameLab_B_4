using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager Instance;

    public GameObject playerPrefab;   // 플레이어 프리팹
    public Transform respawnPoint;    // 리스폰 위치(빈 오브젝트 등으로 설정)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어 생성 함수
    public void SpawnPlayer()
    {
        if (playerPrefab != null && respawnPoint != null)
        {
            Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);
        }
        else
        {

        }
    }

}
