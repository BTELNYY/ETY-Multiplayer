using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteractScript : NetworkBehaviour
{
    [Header("Ray Settings")]
    public float rayDistance;
    public float rayShpereRadius;
    public LayerMask interactableLayer;
    public Transform PlayerCamera;
    public RaycastHit hit;
    [Header("Key Settings")]
    public KeyCode InteractKey = KeyCode.E; //KeyCode for interacting, for settings and stuff.
    [Header("Player Actions")]
    public bool DenyInteractions = false; //use this to prevent people from interacting, e.g. Cuffed, Or in a cutscene.
    [Header("Time Settings")]
    public int MaxUsage = 1;
    public int CooldownFrames = 10;
    //private
    int CurrentUsage;
    int CooldownCounter;
    PlayerScript playerScript;
    NetworkIdentity identity;
    public override void OnStartLocalPlayer()
    {
        playerScript = GetComponent<PlayerScript>();
        PlayerCamera = playerScript.GetPlayerCamera().transform;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 1f, 0);
        identity = playerScript.GetIdentity();
    }
    void Update()
    {
        if (!isLocalPlayer) { return; }
        //meant to prevent interaction spam, eg spamming a door to glitch NPC's or break the door script.
        if (CurrentUsage == MaxUsage)
        {
            CooldownCounter++;
            if (CooldownFrames == CooldownCounter)
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
            CastRay(Camera.main.transform);
        }
    }
    //what ze fuck
    void CastRay(Transform Camera)
    {
        CmdHitDetector(Camera);
    }
    //imagine being me and being this stupid
    void CmdHitDetector(Transform Camera)
    {
        Ray _ray = new Ray(Camera.position, Camera.forward);
        bool _hitSomething = Physics.SphereCast(_ray, rayShpereRadius, out RaycastHit _hitInfo, rayDistance, interactableLayer);
        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
        if (_hitSomething)
        {
            //did it hit the thing
            IInteract _interactable = _hitInfo.transform.GetComponent<IInteract>();
            NetworkIdentity hit_identity = _hitInfo.transform.GetComponent<NetworkIdentity>();
            //hit_identity.AssignClientAuthority(identity.connectionToClient);
            if (_interactable != null)
            {
                CmdInteract(DenyInteractions, _interactable);
            }
            else
            {
                Debug.Log("No Interactable Object Found");
            }
        }
        else
        {
            Debug.Log("Did not hit anything");
        }
    }
    void CmdInteract(bool DenyInteractions, IInteract _interactable)
    {
        if (!DenyInteractions)
        {
            _interactable.interact(playerScript);
        }
        else
        {
            _interactable.interactfail(playerScript);
        }
    }
}
