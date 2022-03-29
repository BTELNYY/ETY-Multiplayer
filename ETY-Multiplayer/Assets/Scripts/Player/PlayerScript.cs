using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Mirror;
using TMPro;
public class PlayerScript : NetworkBehaviour, ITick
{
    [Header("Object Setup")]
    Camera PlayerCamera;
    public Transform Player;
    public NetworkIdentity identity;
    [Header("Player Setup")]
    public string PlayerName = "Player";
    public Color playerColor = Color.white;
    [SyncVar]
    public float PlayerHealth = 100;
    public float PlayerMaxHealth = 100;
    public bool HealthRegen = false;
    public int HealthRegenPerTick = 1;
    [Header("Respawn Settings")]
    public bool CanRespawn = false;
    public Transform RespawnPoint;
    
    //new private members
    DeathTypes lastDamage = DeathTypes.Unknown;
    IDictionary<StatusEffects.StatusEffect, int> CurrentStatusEffects = new Dictionary<StatusEffects.StatusEffect, int>();
    //private members, however these can be returned through a method
    private StatusEffects effect;
    private InventoryScript inventory;
    private MovementScript movement;
    private PlayerInteractScript interact;

    //private members
    public override void OnStartLocalPlayer()
    {
        //this works better then the find object stuff since its really laggy and bad
        Globals.AddITick(this);
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 1f, 0);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PlayerCamera = Camera.main;
        //load the required refrences
        effect = GetComponent<StatusEffects>();
        inventory = GetComponent<InventoryScript>();
        movement = GetComponent<MovementScript>();
        interact = GetComponent<PlayerInteractScript>();

    }












    public void OnDestroy()
    {
        Globals.RemoveITick(this);
    }
    public NetworkIdentity GetIdentity()
    {
        return identity;
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
    }
    public void Kill(DeathTypes death = DeathTypes.Unknown)
    {
        //blah blah death screen
        CurrentStatusEffects.Clear();
        if (CanRespawn)
        {
            Respawn();
        }
        else
        {
            PermaDeath();
        }
    }
    public void Suicide()
    {
        lastDamage = DeathTypes.Suicide;
        Damage(PlayerHealth);
        Kill(lastDamage);
    }
    void PermaDeath()
    {

    }
    public void Respawn()
    {
        Player.position = RespawnPoint.position;
        PlayerHealth = PlayerMaxHealth;
        lastDamage = DeathTypes.Unknown;
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
    public void SetLastDamage(DeathTypes death)
    {
        lastDamage = death;
    }
}
