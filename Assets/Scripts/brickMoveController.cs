using UnityEngine;
using System.Collections;

public class brickMoveController : MonoBehaviour
{
    public float distance = 2f;
    [SerializeField] float WaitTime = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 플레이어와 충돌 시  0.1초 후 로컬 방향으로 이동
            StartCoroutine(MoveBrick());
        }
    }

    IEnumerator MoveBrick()
    {
        yield return new WaitForSeconds(WaitTime);
        transform.Translate(Vector3.forward * distance);
    }
}
