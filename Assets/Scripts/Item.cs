using System.Collections;
using UnityEngine;

public enum ItemType
{
    Default,
    DoubleJump,
    Dash,
    Flash,
    JumpDash,
    JumpFlash,
    DashFlash,
    Triple,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;

    [SerializeField] float itemRespawnTime = 3.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAbility>().GiveAbility(itemType);
            StartCoroutine(RespawnItem());
        }
    }

    IEnumerator RespawnItem()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(itemRespawnTime);
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
