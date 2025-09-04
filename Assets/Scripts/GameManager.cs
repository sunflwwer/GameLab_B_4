using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI; // 버튼 제어
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    // 인스펙터 할당 제거
    private GameObject finalClearPanel;
    private Button homeButton;
    private Button exitButton;

    public int DeathCount { get; private set; } = 0;   // 죽음 횟수
    public float PlayTime { get; private set; } = 0f;  // 현재 씬의 플레이 시간(초)
    


    public bool isRestarting = false;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isRestarting = false;
        isClearing = false;
        PlayTime = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 0) EventSystem 안전장치
        EnsureEventSystem();

        // 1) UI 경로로 패널/버튼 찾기 (Hierarchy 구조 기준)
        finalClearPanel = GameObject.Find("UI/Canvas/ClearPanel");
        if (finalClearPanel != null)
        {
            finalClearPanel.SetActive(false); // 기본은 꺼두기

            var hb = GameObject.Find("UI/Canvas/ClearPanel/HomeButton");
            var eb = GameObject.Find("UI/Canvas/ClearPanel/ExitButton");

            homeButton = hb ? hb.GetComponent<Button>() : null;
            exitButton = eb ? eb.GetComponent<Button>() : null;

            // 2) 리스너 초기화 후 재연결 (중복 방지)
            if (homeButton != null)
            {
                homeButton.onClick.RemoveAllListeners();
                homeButton.onClick.AddListener(OnClickHomeButton);
            }
            if (exitButton != null)
            {
                exitButton.onClick.RemoveAllListeners();
                exitButton.onClick.AddListener(OnClickExitButton);
            }
        }
    }

    private void Update()
    {
        if (!isRestarting && !isClearing)
        {
            PlayTime += Time.deltaTime;
        }
    }

    public void SpawnPlayer()
    {
        if (isRestarting || isClearing) return;

        if (finalClearPanel != null) finalClearPanel.SetActive(false);

        if (UI.Instance != null)
            UI.Instance.ShowFailText("Fail...");

        DeathCount++;
        ExplodePlayer();
        StartCoroutine(RestartCurrentSceneAfterRealtime(2f));
    }

    private void ExplodePlayer()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            effect.TriggerParticle(EffectType.Explosion);
        }
    }

    private IEnumerator RestartCurrentSceneAfterRealtime(float delay)
    {
        isRestarting = true;
        yield return new WaitForSecondsRealtime(delay);


        if (finalClearPanel != null) finalClearPanel.SetActive(false);

       
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void StageClear()
    {
        if (isClearing || isRestarting) return;

        var currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Stage1") StartCoroutine(LoadNextStageAfterRealtime("Stage2", 2f));
        else if (currentScene.name == "Stage2") StartCoroutine(LoadNextStageAfterRealtime("Stage3", 2f));
    }

    private IEnumerator LoadNextStageAfterRealtime(string stageName, float delay)
    {
        isClearing = true;

        if (UI.Instance != null)
            UI.Instance.ShowClearText("Stage Clear!");

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(stageName);
    }

    // 최종 클리어: 여기서만 패널 켬
    public void FinalClear()
    {
        if (isClearing || isRestarting) return;
        isClearing = true;

        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null) playerInput.enabled = false;

        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null) playerController.enabled = false;

        if (finalClearPanel != null) finalClearPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 버튼 핸들러
    public void OnClickHomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }

    public void OnClickExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // EventSystem이 없으면 만들어줌 (Input System 환경 대응)
    private void EnsureEventSystem()
    {
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
#if ENABLE_INPUT_SYSTEM
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
        }
    }
}
