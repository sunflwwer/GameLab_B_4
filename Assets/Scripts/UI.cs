using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI deathCountText;  // UI Text 컴포넌트 연결
    public TextMeshProUGUI timeText;  // UI Text 컴포넌트 연결

    private float playTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;

        string formattedTime = playTime.ToString("F3"); // 소수점 3번재 자리까지 포메팅

        if (GameManager.Instance != null)
        {
            deathCountText.text = " Deaths: " + GameManager.Instance.DeathCount;
            timeText.text = " Time: " + formattedTime;
        }
    }
}

