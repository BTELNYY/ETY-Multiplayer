using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteract
{
    [Header("Ray Cast Settings")]
    public bool IsEnabled = true;
    public bool IsItem = false;
    public virtual void interact(PlayerScript ps)
    {
        Debug.Log("Interacting with " + gameObject.name);
        if (IsItem)
        {
            GetComponentInChildren<ItemBase>().interact(ps);
        }
    }
    public virtual void interactfail(PlayerScript ps)
    {

    }
}


