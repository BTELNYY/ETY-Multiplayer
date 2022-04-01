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
    SyncList<Transform> weaponArray = new SyncList<Transform>(new Transform[5]);
    [SyncVar]
    private ItemBase ItemScript;
    private PlayerScript player;
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced = 1;
    private Transform CurrentTransform;
    void OnWeaponChanged(int _Old, int _New)
    {
        CmdChangeActiveWeapon(_New);
        CmdEquipItem(_Old, _New);
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
    public void ItemManager(bool operation, Transform Item)
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
            CmdDropOrPickupItem(operation, Item, slot);
        }
    }
    public void DropAll()
    {
        int counter = 0;
        while(counter < 5)
        {
            ItemManager(false, weaponArray[counter]);
            counter++;
        }
    }
    //this handles picking up and dropping items
    [Command]
    void CmdDropOrPickupItem(bool operation, Transform Item, int slot)
    {
        if (hasAuthority) 
        {
            Debug.Log(gameObject.name + " has authority to drop or pickup item");
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
            activeWeaponSynced = slot;
        }
        else
        {
            //if the weapon is disabled, dont bother with dropping it
            if(weaponArray[activeWeaponSynced] == null)
            {
                return;
            }
            //make the item be affected by physics
            //there is not getcomponenent cuz there is no need to have one, and item will never drop by default, it must be dropped by a player
            Rigidbody body = weaponArray[activeWeaponSynced].GetComponent<Rigidbody>();
            body.isKinematic = false;
            body.detectCollisions = true;
            body.useGravity = true;
            Item.SetParent(null, true);
            weaponArray[activeWeaponSynced] = null;
        }
    }

    [Command]
    void CmdEquipItem(int _Old, int _New)
    {
        // disable old weapon
        // in range and not null
        if (weaponArray[_Old] != null)
        {
            ItemScript = null;
            weaponArray[_Old].localPosition = new Vector3(weaponArray[_Old].position.x, -100, weaponArray[_Old].position.z);
        }
        // enable new weapon
        // in range and not null
        if (weaponArray[_New] != null)
        {
            ItemScript = weaponArray[_New].GetComponent<ItemBase>();
            weaponArray[_New].localPosition = ItemScript.DefaultSpawnLocation;
        }
        if (weaponArray[_New] == null)
        {
            if (weaponArray[_Old] == null)
            {
                //if you were not holding anything, return.
                return;
            }
            //disable item script if you are not holding anything
            ItemScript = null;
            weaponArray[_Old].localPosition = new Vector3(weaponArray[_Old].position.x, -100, weaponArray[_Old].position.z);
        }
    }
    public int GetFirstEmptySlot()
    {
        int counter = 0;
        foreach (var item in weaponArray)
        {
            if (item == null)
            {
                return counter;
            }
            else
            {
                counter++;
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
            if(weaponArray[activeWeaponSynced] == null)
            {
                //fix for a bug which caused a client disconnect
                return;
            }
            ItemManager(false, weaponArray[activeWeaponSynced]);
        }
        //oddly confusing right? Lists start at 0, the keyboard does not.
        if (Input.GetKeyDown(SlotOne))
        {
            CmdEquipItem(activeWeaponSynced, 0);
        }
        if (Input.GetKeyDown(SlotTwo))
        {
            CmdEquipItem(activeWeaponSynced, 1);
        }
        if (Input.GetKeyDown(SlotThree))
        {
            CmdEquipItem(activeWeaponSynced, 2);
        }
        if (Input.GetKeyDown(SlotFour))
        {
            CmdEquipItem(activeWeaponSynced, 3);
        }
        if (Input.GetKeyDown(SlotFive))
        {
            CmdEquipItem(activeWeaponSynced, 4);
        }
    }
}