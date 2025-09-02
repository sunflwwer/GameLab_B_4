using UnityEngine;

public class brickMoveController : MonoBehaviour
{
    public float distance = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //플레이어와 충돌 후 떨어질때 로컬방향 앞으로 이동
    void OnCollisionExit(Collision collision)
    {
        //bool에서 선택된 로컬방향으로 이동
        if (collision.gameObject.tag == "Player")
        {
         transform.Translate(Vector3.forward * distance);
        }
    }
}
