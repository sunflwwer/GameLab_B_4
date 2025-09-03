using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI deathCountText;  // UI Text ������Ʈ ����
    public TextMeshProUGUI timeText;  // UI Text ������Ʈ ����

    private float playTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;

        string formattedTime = playTime.ToString("F3"); // �Ҽ��� 3���� �ڸ����� ������

        if (GameManager.Instance != null)
        {
            deathCountText.text = " Deaths: " + GameManager.Instance.DeathCount;
            timeText.text = " Time: " + formattedTime;
        }
    }
}

