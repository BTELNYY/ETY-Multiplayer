using System.Collections;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;
public class ItemBase : NetworkBehaviour, IItem, IInteract
{
    public string Name = "None";
    public string Description = "None";
    public bool IsEnabled = true;
    public bool HideInInventory = false;
    public bool CanBePickedUp = true;
    public string IconName;
    public string ItemIDString;
    public int ItemID = 0;
    [SyncVar]
    public int Durability = -1;
    public int Damage = 0;
    public int Healing = 0;
    public ItemUtility.Items Item = ItemUtility.Items.none;
    public Transform ItemObject;
    public Vector3 DefaultSpawnLocation = new Vector3(0, 0, 0);
    public Quaternion DefaultSpawnRotation = new Quaternion(0, 0, 0, 0);
    //private members
    Rigidbody body;
    protected virtual ItemUtility.Items CurrentItem()
    {
        return Item;
    }
    public virtual void interact(PlayerScript ps)
    {
        if (!CanBePickedUp)
        {
            return;
        }
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
            Debug.Log("Removing the rigidbody");
            body = ItemObject.GetComponent<Rigidbody>();
            //disable item physics
            body.isKinematic = true;
            body.detectCollisions = false;
            body.useGravity = false;
            Debug.Log("Setting the trasnforms parent");
            GameObject Player = ps.GetPlayerObject();
            ItemObject.SetParent(Player.transform);
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
        Debug.Log("Dropping " + ItemObject.name + " from slot " + slot);
        invscr.RemoveItem(slot);
        //make the item be affected by physics
        //there is not getcomponenent cuz there is no need to have one, and item will never drop by default, it must be dropped by a player
        body.isKinematic = false;
        body.detectCollisions = true;
        body.useGravity = true;
        ItemObject.SetParent(null, true);
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