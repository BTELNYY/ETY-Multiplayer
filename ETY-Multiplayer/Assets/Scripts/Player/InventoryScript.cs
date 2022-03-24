using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
public class InventoryScript : MonoBehaviour, ITick
{
    [Header("Item and inventory settings")]
    public KeyCode PrimaryUse = KeyCode.Mouse0;
    public KeyCode SecondaryUse = KeyCode.Mouse1;
    public KeyCode InventoryKey = KeyCode.F;
    public KeyCode DropKey = KeyCode.Q;
    //private members
    PlayerScript player;
    IDictionary<int, ItemBase> Inventory = new Dictionary<int, ItemBase>();
    //by default, the player is holding nothing
    ItemBase CurrentItem = ItemUtility.GetItemScript(ItemUtility.Items.none);
    GameObject ItemModel;
    bool[] InventorySlots = new bool[5];
    int TickCounter = 0;
    bool InventoryOpen = false;
    IItem ItemInterface;
    public void Tick()
    {
        //a check to remove item duplicates and other bugs
        if (TickCounter == 100)
        {
            TickCounter = 0;
            foreach (int i in Inventory.Keys)
            {
                if (i > InventorySlots.Length)
                {
                    Inventory.Remove(i);
                }
                else
                {
                    InventorySlots[i] = true;
                }
            }
        }
        else
        {
            TickCounter++;
        }
        if (InventoryOpen)
        {
            OpenInventory();
        }
        if (Input.GetKeyDown(InventoryKey))
        {
            InventoryOpen = !InventoryOpen;
        }
    }
    void Start()
    {
        Globals.AddTickObject(gameObject);
        player = gameObject.GetComponent<PlayerScript>();
    }
    void Update()
    {
        if (!InventoryOpen)
        {
            if (Input.GetKeyDown(PrimaryUse))
            {
                CurrentItem.PrimaryUse(gameObject);
            }
            if (Input.GetKeyDown(SecondaryUse))
            {
                CurrentItem.SecondaryUse(gameObject);
            }
        }
    }
    void OpenInventory()
    {
        //ui code goes here

        //this shit needs to set the current held item btw
        //debug code

        ItemModel = Instantiate(CurrentItem.ItemObject, new Vector3(0, 0, 0), Quaternion.identity);
    }
    public void AddItem(ItemBase item, int slot)
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
    }
}