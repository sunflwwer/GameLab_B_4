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

    private bool isRestarting = false; // 게임 오버 재시작 중복 방지
    private bool isClearing = false; // 스테이지 클리어 중복 방지

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드 직후 공통 초기화
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 혹시 남아있을 수 있는 정지 상태/플래그 초기화
        Time.timeScale = 1f;
        isRestarting = false;
        isClearing = false;

        // 마우스 시점 이동용 커서 잠금(정책에 맞게 조절)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 게임 오버(플레이어 사망) 시 호출 → 시간 정지 후 2초 뒤 재시작(씬 리로드)
    public void SpawnPlayer()
    {
        if (isRestarting || isClearing) return;

        DeathCount++;
        StartCoroutine(RestartCurrentSceneAfterRealtime(2f));
    }

    private IEnumerator RestartCurrentSceneAfterRealtime(float delay)
    {
        isRestarting = true;

        // 1) 게임 시간 정지
        Time.timeScale = 0f;

        // TODO: 게임오버 UI가 있다면 여기서 활성화 (Canvas는 Screen Space - Overlay 권장)

        // 2) 실제 시간 기준 대기
        yield return new WaitForSecondsRealtime(delay);

        // 3) 씬 리로드 (OnSceneLoaded에서 timeScale/커서/플래그 초기화)
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 플레이어가 Clear 레이어에 닿았을 때 호출 (스테이지 진행)
    public void StageClear()
    {
        if (isClearing || isRestarting) return;

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Stage1")
        {
            StartCoroutine(LoadNextStageAfterRealtime("Stage2", 2f));
        }
        else if (currentScene.name == "Stage2")
        {
            StartCoroutine(LoadNextStageAfterRealtime("Stage3", 2f));
        }
        // Stage3 이후는 필요 시 추가
    }

    // 클리어 시: 실제 시간 기준 대기 + 시간 정지 + 다음 씬 로드
    private IEnumerator LoadNextStageAfterRealtime(string stageName, float delay)
    {
        isClearing = true;

        // 1) 연출 동안 게임 시간 정지
        Time.timeScale = 0f;

        // TODO: 클리어 UI/효과가 있다면 여기서 활성화 (Overlay 캔버스 권장)

        // 2) 실제 시간 기준 대기
        yield return new WaitForSecondsRealtime(delay);

        // 3) 다음 씬 로드 (OnSceneLoaded에서 timeScale/커서/플래그 초기화)
        SceneManager.LoadScene(stageName);
    }
}
