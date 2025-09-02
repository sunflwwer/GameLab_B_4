using UnityEngine;

public class DoubleJump : Item
{
    private void Awake()
    {
        itemType = ItemType.DoubleJump;
    }

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("Double Jump Collected!");
    }
}
