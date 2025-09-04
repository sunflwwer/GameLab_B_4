using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI; // 버튼 제어
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool shouldCountTime = false; // 시간 카운트 여부

    // === 추가: 별 카운트 & 리셋 플래그 ===
    public int StarCount { get; private set; } = 0;
    private bool resetStarsOnNextStart = false;

    // 인스펙터 할당 제거
    private GameObject finalClearPanel;
    private Button homeButton;
    private Button exitButton;

    public int DeathCount { get; private set; } = 0;   // 죽음 횟수
    public float PlayTime { get; private set; } = 0f;  // 현재 씬의 플레이 시간(초)



    public bool isRestarting = false;
    public bool isClearing = false;


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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 씬 이름에 따라 시간 리셋/카운트 여부 결정
        var sceneName = scene.name;
        if (sceneName == "Start")
        {
            // Start에서는 시간 리셋 + 카운트 중지
            PlayTime = 0f;
            shouldCountTime = false;

            // === 추가: Stage3 클리어 후 Start로 왔을 때만 별 리셋 ===
            if (resetStarsOnNextStart)
            {
                StarCount = 0;
                resetStarsOnNextStart = false;
            }
        }
        else if (sceneName == "Stage1" || sceneName == "Stage2" || sceneName == "Stage3")
        {
            // 스테이지 진입 시 카운트 시작
            shouldCountTime = true;
        }
        else
        {
            // 기타 씬은 기본적으로 카운트 (필요 시 조정)
            shouldCountTime = true;
        }

        EnsureEventSystem();

        finalClearPanel = GameObject.Find("UI/Canvas/ClearPanel");
        if (finalClearPanel != null)
        {
            finalClearPanel.SetActive(false); // 기본은 꺼두기
            var hb = GameObject.Find("UI/Canvas/ClearPanel/HomeButton");
            var eb = GameObject.Find("UI/Canvas/ClearPanel/ExitButton");
            homeButton = hb ? hb.GetComponent<Button>() : null;
            exitButton = eb ? eb.GetComponent<Button>() : null;

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
        // 카운트 조건: 진행중 && 대기/재시작 아님 && 카운트 허용 상태
        if (shouldCountTime && !isRestarting && !isClearing)
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

        // 바로 카운트 중지되도록 즉시 플래그 설정
        isRestarting = true;

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

    private void NextEffect()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            
            effect.TriggerParticle(EffectType.NextStage);
        }
    }

    private void ClearEffect()
    {
        PlayerEffect effect = FindFirstObjectByType<PlayerEffect>();
        if (effect != null)
        {
            effect.TriggerParticle(EffectType.Clear);
        }
    }

    private IEnumerator RestartCurrentSceneAfterRealtime(float delay)
    {
        isRestarting = true;
        yield return new WaitForSecondsRealtime(delay);

        if (finalClearPanel != null) finalClearPanel.SetActive(false);

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name); // 시간 유지
    }


    public void StageClear()
    {
        if (isClearing || isRestarting) return;

        NextEffect();
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Stage1") StartCoroutine(LoadNextStageAfterRealtime("Stage2", 2f));
        else if (currentScene.name == "Stage2") StartCoroutine(LoadNextStageAfterRealtime("Stage3", 2f));
    }

    private IEnumerator LoadNextStageAfterRealtime(string stageName, float delay)
    {
        isClearing = true;

        if (UI.Instance != null)
            UI.Instance.ShowClearText("Clear!");


        yield return new WaitForSecondsRealtime(delay);

        SceneManager.LoadScene(stageName);
    }

    public void FinalClear()
    {
        if (isClearing || isRestarting) return;
        isClearing = true;

        var playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null) playerInput.enabled = false;

        var playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null) playerController.enabled = false;

        if (UI.Instance != null)
            UI.Instance.ShowClearText("Clear!");


        // === 추가: 다음에 Start 씬 들어갈 때 별 리셋되도록 표시 ===
        resetStarsOnNextStart = true;

        // Stage3 최종 클리어 후, 카운트 중지
        ClearEffect();
        shouldCountTime = false;

        if (finalClearPanel != null) finalClearPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // === 추가: 아이템 획득 API ===
    public void CollectStar(GameObject item)
    {
        StarCount++;
        
        if (item != null)
            Destroy(item);
    }



    // 버튼 핸들러
    public void OnClickHomeButton()
    {
        Time.timeScale = 1f;

        // Start로 갈 때 명시적으로 리셋 + 카운트 중지
        PlayTime = 0f;
        shouldCountTime = false;

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
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
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
