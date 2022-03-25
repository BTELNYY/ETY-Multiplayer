using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
public class ItemBase : MonoBehaviour, IItem
{
    public string Name = "None";
    public string Description = "None";
    public bool HideInInventory = true;
    public string IconName;
    public string ItemIDString;
    public int ItemID = 0;
    public int Durability = -1;
    public int Damage = 0;
    public int Healing = 0;
    public ItemUtility.Items Item = ItemUtility.Items.none;
    public GameObject ItemObject;
    public Vector3 DefaultSpawnLocation = new Vector3(0, 0, 0);
    public Quaternion DefaultSpawnRotation = new Quaternion(0, 0, 0, 0);
    public virtual ItemUtility.Items CurrentItem(){
        return Item;
    }
    public virtual void Start()
    {

    }
    public virtual void Equip(GameObject obj)
    {
        
    }
    public virtual void Unequip(GameObject obj)
    {

    }
    public virtual void PrimaryUse(GameObject obj)
    {

    }
    public virtual void SecondaryUse(GameObject obj)
    {

    }
    public virtual void Cancel(GameObject obj)
    {

    }
    public virtual void Drop(InventoryScript invscr, int slot)
    {

    }
    public virtual void Pickup(InventoryScript invscr, int slot)
    {
        invscr.AddItem(gameObject, invscr.GetFirstEmptySlot());
    }
    public virtual void Delete(InventoryScript invscr, int slot)
    {
        invscr.RemoveItem(slot);
    }
}