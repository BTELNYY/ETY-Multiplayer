using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour
{
    [Header("Ray Settings")]
    public float rayDistance;
    public float rayShpereRadius;
    public LayerMask interactableLayer;
    public Transform m_cam;
    [Header("Key Settings")]
    public KeyCode InteractKey = KeyCode.E; //KeyCode for interacting, for settings and stuff.
    [Header("Player Actions")]
    public bool DenyInteractions = false; //use this to prevent people from interacting, e.g. Cuffed, Or in a cutscene.
    [Header("Time Settings")]
    public int MaxUsage = 1;
    public int CooldownFrames = 10;

    int CurrentUsage;
    int CooldownCounter;
    void Update()
    {
        //meant to prevent interaction spam, eg spamming a door to glitch NPC's or break the door script.
        if (CurrentUsage == MaxUsage)
        {
            CooldownCounter++;
            if(CooldownFrames == CooldownCounter)
            {
                CurrentUsage = 0;
                CooldownCounter = 0;
            }
            else
            {
                return;
            }
        }
        if (Input.GetKey(InteractKey) && !DenyInteractions)
        {
            CurrentUsage++;
            CastRay();
        }
    }

    void CastRay()
    {
        //mghit need a chnge to player position?
        Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.SphereCast(_ray, rayShpereRadius, out _hitInfo, rayDistance, interactableLayer);
        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
        if (_hitSomething)
        {
            //did it hit the thing
            InteractableObject _interactable = _hitInfo.transform.GetComponent<InteractableObject>();


            if (_interactable != null)
            {
                if (_interactable.IsEnabled)
                {
                    _interactable.interact();
                }
                else
                {
                    _interactable.interactfail();
                }



            }
            else
            {
                Debug.Log("No interactible in object");
            }
        }
    }
    public enum InteractFailTypes
    {
        InteractionDisabled,
        AccessDenied,
        ExceptionRaised,
        Locked,
        Unknown,
    }
    public string InteractionFailedToString(InteractFailTypes e)
    {
        switch (e)
        {
            case InteractFailTypes.InteractionDisabled:
                return "Interaction Disabled";
            case InteractFailTypes.AccessDenied:
                return "Access Denied";
            case InteractFailTypes.ExceptionRaised:
                return "Exception Raised";
            case InteractFailTypes.Locked:
                return "Object Locked";
            default:
                return "Unknown";
        }
    }
}
