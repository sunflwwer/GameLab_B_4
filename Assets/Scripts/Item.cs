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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAbility>().GiveAbility(itemType);
            Destroy(gameObject);
        }
    }
}
