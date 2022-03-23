using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour, ITick
{
    [Header("Player Setup")]
    public Transform Player;
    public float PlayerHealth = 100;
    public float PlayerMaxHealth = 100;
    public bool HealthRegen = false;
    public int HealthRegenPerTick = 1;
    [Header("Respawn Settings")]
    public bool CanRespawn = false;
    public Transform RespawnPoint;
    public DeathTypes lastDamage = DeathTypes.Unknown;
    public IDictionary<StatusEffects.StatusEffect, int> CurrentStatusEffects = new Dictionary<StatusEffects.StatusEffect, int>();

    private StatusEffects effect;
    //private members
    void Start()
    {
        //this works better then the find object stuff since its really laggy and bad
        Globals.AddTickObject(gameObject);
        effect = GetComponent<StatusEffects>();
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
        Electrecuted,
    }
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
}
