using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteract
{
    [Header("Ray Cast Settings")]
    public bool IsEnabled = true;
    public virtual void interact()
    {

    }
    public virtual void interactfail()
    {

    }
}


