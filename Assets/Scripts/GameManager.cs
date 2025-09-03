using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int DeathCount { get; private set; } = 0;   // 죽음 횟수
    public float PlayTime { get; private set; } = 0f;  // 현재 씬의 플레이 시간(초)

    private bool isRestarting = false;
    private bool isClearing = false;


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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드 후 공통 초기화 (씬별 시간 리셋 포함)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isRestarting = false;
        isClearing = false;

        // 씬이 바뀔 때마다 시간 0으로 리셋
        PlayTime = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 진행 중일 때만 시간 증가 (정지/클리어 대기 중에는 멈춤)
        if (!isRestarting && !isClearing)
        {
            PlayTime += Time.deltaTime;
        }
    }

    // 게임 오버
    public void SpawnPlayer()
    {
        if (isRestarting || isClearing) return;

        // 죽는 순간 DeathCount +1 (UI는 즉시 +1 상태를 보게 됨)
        DeathCount++;
        StartCoroutine(RestartCurrentSceneAfterRealtime(2f));
    }

    private IEnumerator RestartCurrentSceneAfterRealtime(float delay)
    {
        isRestarting = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay);

        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 스테이지 클리어
    public void StageClear()
    {
        if (isClearing || isRestarting) return;

        var currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Stage1")
        {
            StartCoroutine(LoadNextStageAfterRealtime("Stage2", 2f));
        }
        else if (currentScene.name == "Stage2")
        {
            StartCoroutine(LoadNextStageAfterRealtime("Stage3", 2f));
        }
    }

    private IEnumerator LoadNextStageAfterRealtime(string stageName, float delay)
    {

        isClearing = true;

        // 스테이지 클리어 UI
        if (UI.Instance != null)
            UI.Instance.ShowClearText("Stage Clear!");

        Time.timeScale = 0f;

        // delay 동안 기다리기 (Time.timeScale 0이어도 동작)
        yield return new WaitForSecondsRealtime(delay);

        // 씬 전환 전에 Time.timeScale 복구
        Time.timeScale = 1f;

        // 다음 씬 로드
        SceneManager.LoadScene(stageName);
    }

}
