using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractableObject : MonoBehaviour, IInteract
{
    [Header("Ray Cast Settings")]
    [SyncVar]
    public bool IsEnabled = true;
    [SyncVar]
    public bool IsItem = false;
    public Transform Item;
    private ItemBase Script;
    public virtual void interact(PlayerScript ps)
    {
        if (!IsEnabled)
        {
            return;
        }
        Debug.Log("Interacting with " + gameObject.name);
        if (IsItem)
        {
            Script = Item.GetComponent<ItemBase>();
            Script.interact(ps);
        }
    }
    public virtual void interactfail(PlayerScript ps)
    {

    }
}


