using System.Collections;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance; // 싱글톤 인스턴스

    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI stageOneText;
    public TextMeshProUGUI clearText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 UI 유지
        }
        else
        {
            Destroy(gameObject); // 중복 UI 제거
        }
    }

    void Start()
    {
        StartCoroutine(ShowStageOneText());
    }

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        string formattedTime = gm.PlayTime.ToString("F3");

        deathCountText.text = "Deaths: " + gm.DeathCount;
        timeText.text = "Time: " + formattedTime;
    }

    IEnumerator ShowStageOneText()
    {
        if (stageOneText != null)
        {
            stageOneText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            if (stageOneText != null)
                stageOneText.gameObject.SetActive(false);
        }
    }

    public void ShowClearText(string message)
    {
        if (clearText != null)
        {
            clearText.text = message;
            clearText.gameObject.SetActive(true);
        }
    }
}
