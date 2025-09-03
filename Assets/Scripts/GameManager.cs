using UnityEngine;
using UnityEngine.SceneManagement; // 씬 로드용 네임스페이스

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;   // 플레이어 프리팹 (지금은 사용 안 함)
    public Transform respawnPoint;    // 리스폰 위치 (지금은 사용 안 함)
    public int DeathCount { get; private set; } = 0; // 죽음 횟수

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

    // 플레이어 사망 시 호출 → 씬 리로드
    public void SpawnPlayer()
    {
        DeathCount++;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
