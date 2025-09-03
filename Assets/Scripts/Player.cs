using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public float fallThreshold = -10f; // �� ������ y��ǥ�� �۾����� ���� ó��





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���� üũ
        if (transform.position.y < fallThreshold)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); // �÷��̾� ������Ʈ �ı�
        GameManager.Instance.SpawnPlayer(); // ������ ȣ��
    }
}
