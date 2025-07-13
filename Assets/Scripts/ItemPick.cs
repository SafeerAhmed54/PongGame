using UnityEngine;

public class ItemPick : MonoBehaviour
{
    public ItemData data;

    void Start()
    {
        Debug.Log("Picked up: " + data.itemName + ", Damage: " + data.damage);
        Debug.Log("Picked up: " + data.itemName + ", Damage: " + data.damage);
    }
}
