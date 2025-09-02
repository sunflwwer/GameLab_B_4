using UnityEngine;

public enum ItemType
{
    DoubleJump,
    Dash,
}

public abstract class Item : MonoBehaviour
{
    public ItemType itemType;

    public abstract void ApplyEffect(GameObject player);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.gameObject);
            Destroy(gameObject);
        }
    }
}
