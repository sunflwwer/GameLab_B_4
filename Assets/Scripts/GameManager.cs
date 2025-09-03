using UnityEngine;

/// <summary>
/// 게임 관리 및 플레이어 리스폰 관리 클래스 (싱글톤)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;         // 싱글톤 인스턴스

    public GameObject playerPrefab;             // 플레이어 프리팹
    public Transform respawnPoint;              // 플레이어 리스폰 위치
    public int DeathCount { get; private set; } = 0; // 플레이어 사망 횟수

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);      // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);                // 중복 인스턴스 제거
        }
    }

    /// <summary>
    /// 플레이어를 리스폰 위치에 생성하고 사망 횟수 증가
    /// </summary>
    public void SpawnPlayer()
    {
        if (playerPrefab != null && respawnPoint != null)
        {
            Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation); // 플레이어 생성
            DeathCount++; // 사망 횟수 증가

            // 씬 재실행
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
