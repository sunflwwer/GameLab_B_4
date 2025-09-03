using UnityEngine;
using UnityEngine.SceneManagement; // 씬 로드용 네임스페이스
using System.Collections;

/// <summary>
/// 게임 관리 및 플레이어 리스폰 관리 클래스 (싱글톤)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;   // 플레이어 프리팹 (지금은 사용 안 함)
    public Transform respawnPoint;    // 리스폰 위치 (지금은 사용 안 함)
    public int DeathCount { get; private set; } = 0; // 죽음 횟수

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

    // 플레이어 사망 시 호출 → 씬 리로드
    public void SpawnPlayer()
    {
        DeathCount++;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // 플레이어가 Clear 레이어에 닿았을 때 호출
    public void StageClear()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Stage1")
        {
            StartCoroutine(LoadNextStage("Stage2", 2f));
        }
        else if (currentScene.name == "Stage2")
        {
            StartCoroutine(LoadNextStage("Stage3", 2f));
        }
    }

    // 코루틴으로 일정 시간 후 씬 로드
    private IEnumerator LoadNextStage(string stageName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(stageName);
    }
}
