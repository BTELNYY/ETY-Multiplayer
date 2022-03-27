using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iteminteract : MonoBehaviour, IInteract
{
    [Header("Ray Cast Settings")]
    public bool IsEnabled = true;
    public bool ThisIsItem = false;
    public void interact(PlayerScript ps)
    {
        
    }
    public void interactfail(PlayerScript ps)
    {

    }
}
