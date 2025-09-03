using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI timeText;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        // GameManager가 관리하는 값만 사용
        string formattedTime = gm.PlayTime.ToString("F3"); // 소수점 3자리

        deathCountText.text = "Deaths: " + gm.DeathCount;
        timeText.text = "Time: " + formattedTime;
    }
}
