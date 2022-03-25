﻿using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
public class InventoryScript : MonoBehaviour, ITick
{
    [Header("Item and inventory settings")]
    public KeyCode PrimaryUse = KeyCode.Mouse0;
    public KeyCode SecondaryUse = KeyCode.Mouse1;
    public KeyCode DropKey = KeyCode.T;
    public KeyCode SlotOne = KeyCode.Alpha1;
    public KeyCode SlotTwo = KeyCode.Alpha2;
    public KeyCode SlotThree = KeyCode.Alpha3;
    public KeyCode SlotFour = KeyCode.Alpha4;
    public KeyCode SlotFive = KeyCode.Alpha5;
    //private members
    PlayerScript player;
    //use this when you want to add an item to the inventory, or remove it.
    IDictionary<int, GameObject> Inventory = new Dictionary<int, GameObject>();
    GameObject ItemModel;
    int CurrentSlot;
    bool[] InventorySlots = new bool[5];
    ItemBase ItemScript;
    public void Tick()
    {
        if (Input.GetKeyDown(DropKey))
        {
            if (ItemScript != null)
            {
                ItemScript.Drop(this, CurrentSlot);
            }
        }
    }
    void LoadNewItem(ItemUtility.Items item)
    {
        if (ItemUtility.GetItemScript(item) != null && ItemUtility.GetItem(item) != null)
        {
            ItemModel = Instantiate(ItemUtility.GetItem(item));
            ItemScript = ItemModel.GetComponent<ItemBase>();
            ItemModel.SetActive(true);
            ItemModel.transform.SetParent(transform);
            ItemModel.transform.localPosition = ItemScript.DefaultSpawnLocation;
            ItemModel.transform.localRotation = ItemScript.DefaultSpawnRotation;
            ItemScript.Equip(gameObject);
        }
    }
    void LoadFromInventory(int slot)
    {
        if (Inventory.ContainsKey(slot))
        {
            //tells the object that its being removed
            ItemScript = ItemModel.GetComponent<ItemBase>();
            ItemScript.Unequip(gameObject);
            ItemModel.SetActive(false);
            ItemModel = Inventory[slot];
            ItemModel.SetActive(true);
            ItemModel.transform.SetParent(transform);
            ItemModel.transform.localPosition = ItemScript.DefaultSpawnLocation;
            ItemModel.transform.localRotation = ItemScript.DefaultSpawnRotation;
            //yes, another getcomponent.
            ItemScript = ItemModel.GetComponent<ItemBase>();
            ItemScript.Equip(gameObject);
        }
    }
    void Start()
    {
        ClearItems();
        Globals.AddITick(this);
        player = gameObject.GetComponent<PlayerScript>();
        //by default, the player is holding nothing
        LoadNewItem(ItemUtility.Items.none);
    }
    void Update()
    {
        if (Input.GetKeyDown(PrimaryUse))
        {            
            if (ItemScript != null)
            {
                ItemScript.PrimaryUse(gameObject);
            }
        }
        if (Input.GetKeyDown(SecondaryUse))
        {
            if (ItemScript != null)
            {
                ItemScript.SecondaryUse(gameObject);
            }
        }
        if (Input.GetKeyDown(SlotOne))
        {
            CurrentSlot = 1;
            LoadFromInventory(1);
        }
        if (Input.GetKeyDown(SlotTwo))
        {
            CurrentSlot = 2;
            LoadFromInventory(2);
        }
        if (Input.GetKeyDown(SlotThree))
        {
            CurrentSlot = 3;
            LoadFromInventory(3);
        }
        if (Input.GetKeyDown(SlotFour))
        {
            CurrentSlot = 4;
            LoadFromInventory(4);
        }
        if (Input.GetKeyDown(SlotFive))
        {
            CurrentSlot = 5;
            LoadFromInventory(5);
        }
    }
    public void AddItem(GameObject item, int slot)
    {
        //fail safe to prevent items from just being deleted for no reason
        if (InventorySlots[slot])
        {
            if (CheckFullInventory())
            {
                return;
            }
            else
            {
                slot = GetFirstEmptySlot();
            }
        }
        Inventory.Add(slot, item);
    }
    public bool CheckFullInventory()
    {
        int counter = 0;
        foreach (bool i in InventorySlots)
        {
            if (i)
            {
                counter++;
            }
        }
        if (counter == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetFirstEmptySlot()
    {
        foreach (int i in Inventory.Keys)
        {
            if (!InventorySlots[i])
            {
                return i;
            }
        }
        return 0;
    }
    public void RemoveItem(int slot)
    {
        Inventory.Remove(slot);
    }
    public void ClearItems()
    {
        Inventory.Clear();
        Inventory.Add(1, null);
        Inventory.Add(2, null);
        Inventory.Add(3, null);
        Inventory.Add(4, null);
        Inventory.Add(5, null);
    }
}