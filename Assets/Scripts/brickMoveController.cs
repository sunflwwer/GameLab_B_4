using UnityEngine;

public class brickMoveController : MonoBehaviour
{
    public float distance = 1f;
    private bool stopped = false;

    void OnCollisionExit(Collision collision)
    {
        if (!stopped && collision.gameObject.CompareTag("Player"))
        {
            // 앞으로 Raycast 쏴서 막혀 있으면 이동 안함
            if (!Physics.Raycast(transform.position, transform.forward, distance))
            {
                transform.Translate(Vector3.forward * distance);
            }
            else
            {
                stopped = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            stopped = true;
        }
    }
}
