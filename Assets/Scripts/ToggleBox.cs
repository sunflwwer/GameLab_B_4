using UnityEngine;
using System.Collections;

public class ToggleBox : MonoBehaviour
{
    [SerializeField] private float onTime = 1f;      // ���� �ִ� �ð�
    [SerializeField] private float offTime = 2f;     // ���� �ִ� �ð�
    [SerializeField] private float startDelay = 0f;  // ���� ���� (�ڽ����� �ٸ��� ����)
    [SerializeField] private float hiddenAlpha = 0.1f; // �������� �� ���� �� (0=��������, 1=������)

    private Renderer[] renderers;
    private Collider col;
    private Material[] materials;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        col = GetComponent<Collider>();

        // ��Ƽ���� �迭 ����
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }

    void Start()
    {
        StartCoroutine(ToggleRoutine());
    }

    IEnumerator ToggleRoutine()
    {
        // ������ �����̸�ŭ ��ٷȴٰ� ����
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // �ѱ� (���� 1, �ݶ��̴� �ѱ�)
            SetVisible(true);
            yield return new WaitForSeconds(onTime);

            // ���� (���� ���߰�, �ݶ��̴� ����)
            SetVisible(false);
            yield return new WaitForSeconds(offTime);
        }
    }

    void SetVisible(bool visible)
    {
        if (visible)
        {
            foreach (var mat in materials)
            {
                Color c = mat.color;
                c.a = 1f;         // ���� ���̰�
                mat.color = c;
            }
            if (col != null) col.enabled = true;
        }
        else
        {
            foreach (var mat in materials)
            {
                Color c = mat.color;
                c.a = hiddenAlpha; // ������
                mat.color = c;
            }
            if (col != null) col.enabled = false;
        }
    }
}
