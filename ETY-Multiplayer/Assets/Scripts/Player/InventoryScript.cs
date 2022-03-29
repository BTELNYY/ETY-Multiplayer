using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Mirror;
public class InventoryScript : NetworkBehaviour
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
    private int selectedWeaponLocal = 1;
    [SyncVar]
    public Transform[] weaponArray = new Transform[5];
    [SyncVar]
    private ItemBase ItemScript;
    private PlayerScript player;
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced = 1;
    private Transform CurrentTransform;
    void OnWeaponChanged(int _Old, int _New)
    {
        // disable old weapon
        // in range and not null
        if (0 < _Old && _Old < weaponArray.Length && weaponArray[_Old] != null)
        {
            weaponArray[_Old].position = new Vector3(weaponArray[_Old].position.x, -100, weaponArray[_Old].position.z);
        }
        // enable new weapon
        // in range and not null
        if (0 < _New && _New < weaponArray.Length && weaponArray[_New] != null)
        {
            ItemScript = weaponArray[_New].GetComponent<ItemBase>();
            weaponArray[_New].position = new Vector3(weaponArray[_Old].position.x, -100, weaponArray[_Old].position.z);
        }

    }
    void ShowHand(bool YeetIntoSpace, Transform item)
    {
        if (YeetIntoSpace)
        {
            item.position = new Vector3(item.position.x, -100, item.position.z);
        }
        else
        {
            item.position = new Vector3(item.position.x, -100, item.position.z);
        }
    }

    [Command]
    public void CmdChangeActiveWeapon(int newIndex)
    {
        activeWeaponSynced = newIndex;
    }

    void Awake()
    {
        // disable all weapons
        foreach (var item in weaponArray)
        {
            if (item != null)
            {
                item.position = new Vector3(item.position.x, -100, item.position.z);
            }
        }
        CurrentTransform = transform;
    }
    public void ItemManager(bool operation, Transform Item, int Slot)
    {
        //runs locally
        int slot = GetFirstEmptySlot();
        if (slot == -1)
        {
            Debug.Log("No empty slots");
            return;
        }
        else
        {
            ServerItemManager(operation, Item, slot);
        }
    }
    [Command]
    void ServerItemManager(bool operation, Transform Item, int slot)
    {
        if(slot == 0)
        {
            foreach (var item in weaponArray)
            {
                if (item == Item) //if the item 
                {
                    
                }
            }
        }
        //function runs on server
        if (operation)
        {
            weaponArray[slot] = Item;
            //duplicate, its fine
            ItemScript = Item.GetComponent<ItemBase>();
            Item.SetParent(transform);
            Debug.Log("Removing the rigidbody");
            Rigidbody body = Item.GetComponent<Rigidbody>();
            //disable item physics
            body.isKinematic = true;
            body.detectCollisions = false;
            body.useGravity = false;
            Debug.Log("Setting the trasnforms parent");
            Item.transform.SetParent(transform);
            Debug.Log("Setting the transforms local position");
            Item.localPosition = ItemScript.DefaultSpawnLocation;
            Item.localRotation = ItemScript.DefaultSpawnRotation;
        }
        else
        {
            weaponArray[slot] = null;
            //make the item be affected by physics
            //there is not getcomponenent cuz there is no need to have one, and item will never drop by default, it must be dropped by a player
            Rigidbody body = weaponArray[slot].GetComponent<Rigidbody>();
            body.isKinematic = false;
            body.detectCollisions = true;
            body.useGravity = true;
            Item.SetParent(null, true);
        }
    }
    public int GetFirstEmptySlot()
    {
        foreach (var item in weaponArray)
        {
            if (item == null)
            {
                return Array.IndexOf(weaponArray, item);
            }
            else
            {
                continue;
            }
        }
        //no empty slot
        return -1;
    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
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
                ItemScript.Drop(this, selectedWeaponLocal);
            }
        }
        if (Input.GetKeyDown(SlotOne))
        {
            CmdChangeActiveWeapon(0);
        }
        if (Input.GetKeyDown(SlotTwo))
        {
            CmdChangeActiveWeapon(1);
        }
        if (Input.GetKeyDown(SlotThree))
        {
            CmdChangeActiveWeapon(2);
        }
        if (Input.GetKeyDown(SlotFour))
        {
            CmdChangeActiveWeapon(3);
        }
        if (Input.GetKeyDown(SlotFive))
        {
            CmdChangeActiveWeapon(4);
        }
    }
}


























//old code

//    [Header("Item and inventory settings")]
//    public KeyCode PrimaryUse = KeyCode.Mouse0;
//    public KeyCode SecondaryUse = KeyCode.Mouse1;
//    public KeyCode DropKey = KeyCode.T;
//    public KeyCode SlotOne = KeyCode.Alpha1;
//    public KeyCode SlotTwo = KeyCode.Alpha2;
//    public KeyCode SlotThree = KeyCode.Alpha3;
//    public KeyCode SlotFour = KeyCode.Alpha4;
//    public KeyCode SlotFive = KeyCode.Alpha5;
//    //private members
//    PlayerScript player;
//    //use this when you want to add an item to the inventory, or remove it.
//    IDictionary<int, Transform> Inventory = new SyncDictionary<int, Transform>();
//    [SyncVar]
//    Transform ItemModel;
//    [SyncVar]
//    int CurrentSlot;
//    bool[] InventorySlots = new bool[5];
//    ItemBase ItemScript;
//    public void Tick()
//    {

//    }
//    void LoadNewItem(ItemUtility.Items item)
//    {
//        if (ItemUtility.GetItemScript(item) != null && ItemUtility.GetItem(item) != null)
//        {
//            //TODO: fix later
//            //ItemModel = Instantiate(ItemUtility.GetItem(item));
//            ItemScript = ItemModel.GetComponent<ItemBase>();
//            //ItemModel.SetActive(true);
//            ItemModel.transform.SetParent(transform);
//            ItemModel.transform.localPosition = ItemScript.DefaultSpawnLocation;
//            ItemModel.transform.localRotation = ItemScript.DefaultSpawnRotation;
//            ItemScript.Equip(player);
//        }
//    }
//    [Command]
//    void LoadFromInventory(int slot)
//    {
//        Debug.Log("Loading from inventory slot: " + slot);
//        if (Inventory.ContainsKey(slot))
//        {
//            //tells the object that its being removed
//            if (ItemModel != null)
//            {
//                ItemScript = ItemModel.GetComponent<ItemBase>();
//                ItemScript.Unequip(player);
//                ItemModel.gameObject.SetActive(false);
//            }
//            Debug.Log("Itemodel: " + ItemModel);
//            ItemModel = Inventory[slot];
//            ItemScript = ItemModel.GetComponent<ItemBase>();
//            ItemModel.gameObject.SetActive(true);
//            ItemModel.SetParent(transform);
//            ItemModel.localPosition = ItemScript.DefaultSpawnLocation;
//            ItemModel.localRotation = ItemScript.DefaultSpawnRotation;
//            //yes, another getcomponent.
//            ItemScript.SanityCheck(); //a sanity check to make sure the item is still valid, and that I am not going insane
//            ItemScript.Equip(player);
//            CurrentSlot = slot;
//        }
//        else
//        {
//            Debug.Log("No item in slot: " + slot + "Calling show arm function");
//            ShowHand(false);
//        }
//    }
//    [Command]
//    void ShowHand(bool doNotYeetIntoSpace)
//    {
//        if(ItemModel == null)
//        {
//            Debug.Log("No item in hand");
//            return;
//        }
//        else
//        {
//            if (!doNotYeetIntoSpace)
//            {
//                ItemScript = null;
//                ItemModel.localPosition = new Vector3(ItemModel.localPosition.x, 1000, ItemModel.localPosition.z);
//            }
//            else
//            {
//                //used for dropping the item
//                ItemScript = null;
//                ItemModel.localPosition = new Vector3(ItemModel.localPosition.x, ItemModel.position.y, ItemModel.localPosition.z + 0.5f);
//            }
//        }
//    }
//    void Start()
//    {
//        Globals.AddITick(this);
//        player = gameObject.GetComponent<PlayerScript>();
//        Inventory.Clear(); //remove all items from inv, just in case
//    }
//    public void AddItem(Transform item, int slot)
//    {
//        //fail safe to prevent items from just being deleted for no reason
//        if (InventorySlots[slot])
//        {
//            if (CheckFullInventory())
//            {
//                return;
//            }
//            else
//            {
//                slot = GetFirstEmptySlot();
//            }
//        }
//        item.SetParent(transform); //attach the object to the player
//        Inventory.Add(slot, item);
//        LoadFromInventory(slot);
//    }
//    public bool CheckFullInventory()
//    {
//        int counter = 0;
//        foreach (bool i in InventorySlots)
//        {
//            if (i)
//            {
//                counter++;
//            }
//        }
//        if (counter == 5)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    public int GetFirstEmptySlot()
//    {
//        foreach (int i in Inventory.Keys)
//        {
//            if (!InventorySlots[i])
//            {
//                return i;
//            }
//        }
//        return 0;
//    }
//    public void RemoveItem(int slot)
//    {
//        Inventory.Remove(slot);
//        ShowHand(true);
//        //remove the refrence to the item model
//        ItemModel = null;
//    }
//}