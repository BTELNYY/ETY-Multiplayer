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
        inv.ItemManager(true, ItemObject);

        
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
    public virtual void Delete(InventoryScript invscr, int slot)
    {

    }
}