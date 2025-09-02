using UnityEngine;

public class Dash : Item
{
    private void Awake()
    {
        itemType = ItemType.Dash;
    }

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("Dash Collected!");
    }
}
