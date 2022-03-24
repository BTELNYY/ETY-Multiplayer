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
    public string ItemID;
    public int Durability = -1;
    public int Damage = 0;
    public int Healing = 0;
    public ItemUtility.Items Item = ItemUtility.Items.none;
    public GameObject ItemObject;
    public virtual ItemUtility.Items CurrentItem(){
        return Item;
    }
    public virtual void Start()
    {
        //this needs to be virtual so that any classes which inhert this will atually load the properties of the item correctly
        ItemObject = ItemUtility.GetGameObject(Item);
        IconName = ItemUtility.GetItemID(Item);
        ItemID = ItemUtility.GetItemID(Item);
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
    public virtual void Drop(InventoryScript invscr)
    {

    }
    public virtual void Pickup(InventoryScript invscr)
    {

    }
    public virtual void Delete(InventoryScript invscr)
    {
        
    }
}