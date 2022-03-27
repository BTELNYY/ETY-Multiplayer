using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteract
{
    [Header("Ray Cast Settings")]
    public bool IsEnabled = true;
    public bool ThisIsItem = false;

    public virtual void interact(PlayerScript ps)
    {
        if (ThisIsItem)
        {
            //fuck this shit man
            gameObject.transform.GetComponent<ItemBase>().interact(ps);
        }
    }
    public virtual void interactfail(PlayerScript ps)
    {

    }
}


