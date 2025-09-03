using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager Instance;

    public GameObject playerPrefab;   // �÷��̾� ������
    public Transform respawnPoint;    // ������ ��ġ(�� ������Ʈ ������ ����)

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

    // �÷��̾� ���� �Լ�
    public void SpawnPlayer()
    {
        if (playerPrefab != null && respawnPoint != null)
        {
            Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);
        }
        else
        {

        }
    }

}
