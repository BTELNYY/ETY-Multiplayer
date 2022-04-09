using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Mirror;
public class PlayerScript : NetworkBehaviour, ITick
{
    [Header("Object Setup")]
    Camera PlayerCamera;
    public Transform Player;
    NetworkIdentity identity;
    [Header("Camera Setup")]
    public float CameraDefaultX = 0f;
    public float CameraDefaultY = 0f;
    public float CameraDefaultZ = 0f;
    [Header("Player Setup")]
    public string CharacterName = "Ayano Aishi";
    public Color playerNameColor = Color.white;
    [SyncVar]
    public float PlayerHealth = 100;
    [SyncVar]
    public float PlayerMaxHealth = 100;
    [SyncVar]
    public bool HealthRegen = false;
    [SyncVar]
    public int HealthRegenPerTick = 1;
    [Header("Respawn Settings")]
    public bool CanRespawn = false;
    //new private members
    private GameObject CurrentClass;
    DeathTypes lastDamage = DeathTypes.Unknown;
    IDictionary<StatusEffects.StatusEffect, int> CurrentStatusEffects = new Dictionary<StatusEffects.StatusEffect, int>();
    //private members, however these can be returned through a method
    private StatusEffects effect;
    private InventoryScript inventory;
    private MovementScript movement;
    private PlayerInteractScript interact;
    private ClassBase classBase;
    void Start()
    {
        //this works better then the find object stuff since its really laggy and bad
        Globals.AddITick(this);
        //yes this code is copied, I dont care, I want it to work.
        if (Camera.main == null)
        {
            //makes a new camera if none exists
            GameObject cam_obj = new GameObject("Camera");
            cam_obj.AddComponent<Camera>();
            cam_obj.tag = "MainCamera";
        }
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(CameraDefaultX, CameraDefaultY, CameraDefaultZ);
        PlayerCamera = Camera.main;
        //load the required refrences
        effect = GetComponent<StatusEffects>();
        inventory = GetComponent<InventoryScript>();
        movement = GetComponent<MovementScript>();
        interact = GetComponent<PlayerInteractScript>();
        classBase = GetComponent<ClassBase>();
    }
    public Vector3 CameraPosition() 
    {
        return new Vector3(CameraDefaultX, CameraDefaultY, CameraDefaultZ);
    }
    public void RemoveTick()
    {
        Globals.RemoveITick(this);
    }
    public InventoryScript GetInventory()
    {
        return inventory;
    }
    public GameObject GetPlayerObject()
    {
        return gameObject;
    }
    public Transform GetPLayerTransform()
    {
        return Player;
    }
    public Camera GetPlayerCamera()
    {
        return PlayerCamera;
    }
    public void Tick()
    {
        //tick
        effect.Tick();
    }
    void Update()
    {
        if (PlayerHealth == 0f)
        {
            Kill(lastDamage);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Suicide();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void Kill(DeathTypes death = DeathTypes.Unknown)
    {
        //blah blah death screen
        ClassGlobals classes = ClassGlobals.Instance;
        CurrentStatusEffects.Clear();
        if (CanRespawn)
        {
            if(classes.DebugPlayer == null)
            {
                Debug.Log("debug player is null v2");
            }
            inventory.DropAll();
            RemoveTick();
            CmdRespawn(gameObject, classes.DebugPlayer);
        }
        else
        {
            CmdPermaDeath();
        }
    }
    public void Suicide()
    {
        lastDamage = DeathTypes.Suicide;
        Damage(PlayerHealth);
        Kill(lastDamage);
    }
    public void Damage(float damage)
    {
        if (PlayerHealth - damage < 0)
        {
            PlayerHealth = 0;
            Debug.Log("Player has died!");
            Kill(lastDamage);
        }
        else
        {
            PlayerHealth -= damage;
            Debug.Log("Dealt: " + damage.ToString() + " damage to the player.");
        }
    }
    public void Heal(float health, bool allowoverheal)
    {
        if (allowoverheal)
        {
            PlayerHealth += health;
        }
        else
        {
            if (PlayerHealth + health > 100)
            {
                PlayerHealth = 100;
            }
        }
    }
    public void SetHealth(float health)
    {
        PlayerHealth = health;
    }
    public enum DeathTypes
    {
        Suicide,
        Burning,
        Poison,
        Unknown,
        Bleeding,
        GameOver,
        Attacked,
        FallDamage,
    }
    public void SetLastDamage(DeathTypes death)
    {
        lastDamage = death;
    }
    public void SwitchClass(GameObject new_class)
    {
        ClassGlobals classes = ClassGlobals.Instance;
        if(classes.DebugPlayer == null)
        {
            Debug.Log("Debug Player is null");
        }
        inventory.DropAll();
        RemoveTick();
        CmdRespawn(gameObject, classes.DebugPlayer);
    }
    [Command]
    public void CmdRespawn(GameObject obj, GameObject new_class)
    {
        ClassGlobals classes = ClassGlobals.Instance;
        identity = GetComponent<NetworkIdentity>();
        if(new_class == null)
        {
            Debug.Log("No class was given to respawn with!");
            return;
        }
        NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, Instantiate(new_class));
        identity.SendMessage("Start");
        Destroy(obj);
    }
    [Command]
    void CmdPermaDeath()
    {

    }
    //the purpose of these methods is to remove the dict from being public, as that would cause a lot of issues
    public void RemoveEffect(StatusEffects.StatusEffect effect)
    {
        CurrentStatusEffects.Remove(effect);
    }
    public void AddEffect(StatusEffects.StatusEffect effect, int duration)
    {
        if (CurrentStatusEffects.ContainsKey(effect))
        {
            CurrentStatusEffects[effect] += duration;
        }
        else
        {
            CurrentStatusEffects.Add(effect, duration);
        }
    }
    public bool HasEffect(StatusEffects.StatusEffect effect)
    {
        return CurrentStatusEffects.ContainsKey(effect);
    }
    public int GetEffectDuration(StatusEffects.StatusEffect effect)
    {
        if (CurrentStatusEffects.ContainsKey(effect))
        {
            return CurrentStatusEffects[effect];
        }
        else
        {
            return 0;
        }
    }
    public void ReduceEffectDuration(StatusEffects.StatusEffect effect, int amount)
    {
        if (CurrentStatusEffects.ContainsKey(effect))
        {
            CurrentStatusEffects[effect] = -amount;
        }
    }
}
