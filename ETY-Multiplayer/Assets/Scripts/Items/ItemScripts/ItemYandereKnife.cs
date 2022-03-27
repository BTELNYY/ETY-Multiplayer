using System.Collections;
using UnityEngine;

public class ItemYandereKnife : ItemBase
{
    public override void interact(PlayerScript ps)
    {
        base.interact(ps);
        /*
        InventoryScript inv = ps.GetInventory();
        if (inv.CheckFullInventory())
        {
            Debug.Log(ps.PlayerName + "'s inventory is full");
            //the inventory is full.
            return;
        }
        else
        {
            int slot = inv.GetFirstEmptySlot();
            //Debug.Log("Adding Item " + ItemObject.name + " to slot " + slot);
            inv.AddItem(ItemObject, slot);
            Debug.Log("Setting the trasnforms parent");
            GameObject Player = ps.GetPlayerObject();
            ItemObject.SetParent(Player.transform);
            Debug.Log("Destroying the old object");
        }
        */
    }
    public override void PrimaryUse(PlayerScript ps)
    {
        base.PrimaryUse(ps);
        Debug.Log("PrimaryUse");
    }
    public override void SecondaryUse(PlayerScript ps)
    {
        base.SecondaryUse(ps);
        Debug.Log("SecondaryUse");
    }
}