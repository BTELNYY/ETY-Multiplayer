using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Mirror;
public class InventoryScript : NetworkBehaviour, ITick
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
    IDictionary<int, Transform> Inventory = new SyncDictionary<int, Transform>();
    [SyncVar]
    Transform ItemModel;
    [SyncVar]
    int CurrentSlot;
    bool[] InventorySlots = new bool[5];
    ItemBase ItemScript;
    public void Tick()
    {

    }
    void LoadNewItem(ItemUtility.Items item)
    {
        if (ItemUtility.GetItemScript(item) != null && ItemUtility.GetItem(item) != null)
        {
            //TODO: fix later
            //ItemModel = Instantiate(ItemUtility.GetItem(item));
            ItemScript = ItemModel.GetComponent<ItemBase>();
            //ItemModel.SetActive(true);
            ItemModel.transform.SetParent(transform);
            ItemModel.transform.localPosition = ItemScript.DefaultSpawnLocation;
            ItemModel.transform.localRotation = ItemScript.DefaultSpawnRotation;
            ItemScript.Equip(player);
        }
    }
    void LoadFromInventory(int slot)
    {
        Debug.Log("Loading from inventory slot: " + slot);
        if (Inventory.ContainsKey(slot))
        {
            //tells the object that its being removed
            if (ItemModel != null)
            {
                ItemScript = ItemModel.GetComponent<ItemBase>();
                ItemScript.Unequip(player);
                ItemModel.gameObject.SetActive(false);
            }
            Debug.Log("Itemodel: " + ItemModel);
            ItemModel = Inventory[slot];
            ItemScript = ItemModel.GetComponent<ItemBase>();
            ItemModel.gameObject.SetActive(true);
            ItemModel.SetParent(transform);
            ItemModel.localPosition = ItemScript.DefaultSpawnLocation;
            ItemModel.localRotation = ItemScript.DefaultSpawnRotation;
            //yes, another getcomponent.
            ItemScript.SanityCheck(); //a sanity check to make sure the item is still valid, and that I am not going insane
            ItemScript.Equip(player);
            CurrentSlot = slot;
        }
        else
        {
            Debug.Log("No item in slot: " + slot + "Calling show arm function");
            ShowHand(false);
        }
    }
    void ShowHand(bool doNotYeetIntoSpace)
    {
        if(ItemModel == null)
        {
            Debug.Log("No item in hand");
            return;
        }
        else
        {
            if (!doNotYeetIntoSpace)
            {
                ItemScript = null;
                ItemModel.localPosition = new Vector3(ItemModel.localPosition.x, 1000, ItemModel.localPosition.z);
            }
            else
            {
                //used for dropping the item
                ItemScript = null;
                ItemModel.localPosition = new Vector3(ItemModel.localPosition.x, ItemModel.position.y, ItemModel.localPosition.z + 0.5f);
            }
        }
    }
    void Start()
    {
        Globals.AddITick(this);
        player = gameObject.GetComponent<PlayerScript>();
        Inventory.Clear(); //remove all items from inv, just in case
    }
    void Update()
    {
        if (Input.GetKeyDown(PrimaryUse))
        {            
            if (ItemScript != null)
            {
                ItemScript.PrimaryUse(player);
            }
            else
            {
                Debug.Log("ItemScript is null");
            }
        }
        if (Input.GetKeyDown(SecondaryUse))
        {
            if (ItemScript != null)
            {
                ItemScript.SecondaryUse(player);
            }
            else
            {
                Debug.Log("ItemScript is null");
            }
        }
        if (Input.GetKeyDown(DropKey))
        {
            if (ItemScript != null)
            {
                ItemScript.Drop(this, CurrentSlot);
            }
        }
        if (Input.GetKeyDown(SlotOne))
        {
            CurrentSlot = 0;
            LoadFromInventory(0);
        }
        if (Input.GetKeyDown(SlotTwo))
        {
            CurrentSlot = 1;
            LoadFromInventory(1);
        }
        if (Input.GetKeyDown(SlotThree))
        {
            CurrentSlot = 2;
            LoadFromInventory(2);
        }
        if (Input.GetKeyDown(SlotFour))
        {
            CurrentSlot = 3;
            LoadFromInventory(3);
        }
        if (Input.GetKeyDown(SlotFive))
        {
            CurrentSlot = 4;
            LoadFromInventory(4);
        }
    }
    public void AddItem(Transform item, int slot)
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
        item.SetParent(transform); //attach the object to the player
        Inventory.Add(slot, item);
        LoadFromInventory(slot);
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
        ShowHand(true);
        //remove the refrence to the item model
        ItemModel = null;
    }
}