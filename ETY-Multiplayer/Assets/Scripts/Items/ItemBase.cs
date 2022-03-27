using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
public class ItemBase : MonoBehaviour, IItem, IInteract
{
    public string Name = "None";
    public string Description = "None";
    public bool IsEnabled = true;
    public bool HideInInventory = false;
    public string IconName;
    public string ItemIDString;
    public int ItemID = 0;
    public int Durability = -1;
    public int Damage = 0;
    public int Healing = 0;
    public ItemUtility.Items Item = ItemUtility.Items.none;
    public Transform ItemObject;
    public Vector3 DefaultSpawnLocation = new Vector3(0, 0, 0);
    public Quaternion DefaultSpawnRotation = new Quaternion(0, 0, 0, 0);
    protected virtual ItemUtility.Items CurrentItem()
    {
        return Item;
    }
    public virtual void interact(PlayerScript ps)
    {
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
            Debug.Log("Adding Item " + ItemObject.name + " to slot " + slot);
            inv.AddItem(ItemObject, slot);
            Debug.Log("Setting the trasnforms parent");
            GameObject Player = ps.GetPlayerObject();
            ItemObject.SetParent(Player.transform);
            Debug.Log("Destroying the old object");
        }
    }
    public virtual void SanityCheck()
    {
        Debug.Log(ItemObject.name + ": Sanity Check");
    }
    public virtual void interactfail(PlayerScript ps)
    {

    }
    public virtual void Equip(PlayerScript ps)
    {
        
    }
    public virtual void Unequip(PlayerScript ps)
    {

    }
    public virtual void PrimaryUse(PlayerScript ps)
    {

    }
    public virtual void SecondaryUse(PlayerScript ps)
    {

    }
    public virtual void Cancel(PlayerScript ps)
    {

    }
    public virtual void Drop(InventoryScript invscr, int slot)
    {

    }
    public virtual void Pickup(InventoryScript invscr, int slot)
    {
        invscr.AddItem(ItemObject, invscr.GetFirstEmptySlot());
    }
    public virtual void Delete(InventoryScript invscr, int slot)
    {
        invscr.RemoveItem(slot);
    }
}