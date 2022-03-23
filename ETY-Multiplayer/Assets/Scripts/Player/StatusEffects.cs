using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    int currentcount;
    PlayerScript PlayerScript;
    void Start()
    {
        //fuck you, I dont care about bad game bullshit, this works in the end.
        PlayerScript = transform.GetComponent<PlayerScript>();
    }
    void Update()
    {

    }
    void EffectHandle(object sender, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("About to calculate status effects");
        //write code to handle each effect
        foreach (StatusEffect se in (StatusEffect[])Enum.GetValues(typeof(StatusEffect)))
        {
            switch (se)
            {
                case StatusEffect.None:
                    break;
                case StatusEffect.Burning:
                    EffectBurning();
                    break;
                case StatusEffect.Poison:
                    EffectPoison();
                    break;
                case StatusEffect.Suffocating:
                    break;
                case StatusEffect.Regeneration:
                    EffectRegen();
                    break;
                case StatusEffect.God:
                    EffectGod();
                    break;
                case StatusEffect.NoFly:
                    break;
                default:
                    break;
            }
        }
    }
    void EffectGod()
    {
        StatusEffect se = StatusEffect.God;
        if (PlayerScript.CurrentStatusEffects.ContainsKey(se))
        {
            if (PlayerScript.CurrentStatusEffects[se] > 0)
            {
                PlayerScript.SetHealth(-1); //change line to do something different in a differnet method or otherwise.
                PlayerScript.CurrentStatusEffects[se] -= 1;
            }
            else
            {
                RemoveEffect(se);
            }
        }
    }
    void EffectPoison()
    {
        if (PlayerScript.CurrentStatusEffects.ContainsKey(StatusEffect.Poison))
        {
            if (PlayerScript.CurrentStatusEffects[StatusEffect.Poison] > 0)
            {
                PlayerScript.lastDamage = PlayerScript.DeathTypes.Poison;
                PlayerScript.Damage(0.01f); //change line to do something different in a differnet method or otherwise.
                PlayerScript.CurrentStatusEffects[StatusEffect.Poison] -= 1;
            }
            else
            {
                RemoveEffect(StatusEffect.Poison);
            }
        }
    }
    void EffectRegen()
    {
        StatusEffect se = StatusEffect.Regeneration;
        if (PlayerScript.CurrentStatusEffects.ContainsKey(se))
        {
            if (PlayerScript.CurrentStatusEffects[se] > 0)
            {
                PlayerScript.Heal(0.1f, false); //change line to do something different in a differnet method or otherwise.
                PlayerScript.CurrentStatusEffects[se] -= 1;
            }
            else
            {
                RemoveEffect(se);
            }
        }
    }
    void EffectBurning()
    {
        StatusEffect se = StatusEffect.Burning;
        if (PlayerScript.CurrentStatusEffects.ContainsKey(se))
        {
            if (PlayerScript.CurrentStatusEffects[se] > 0)
            {
                PlayerScript.lastDamage = PlayerScript.DeathTypes.Burning;
                PlayerScript.Damage(0.05f); //change line to do something different in a differnet method or otherwise.
                PlayerScript.CurrentStatusEffects[se] -= 1;
            }
            else
            {
                RemoveEffect(se);
            }
        }
    }
    public enum StatusEffect
    {
        None,
        Burning,
        Poison,
        Suffocating,
        Regeneration,
        God,
        NoFly,
    }
    public void RemoveEffect(StatusEffect effect)
    {
        PlayerScript.CurrentStatusEffects.Remove(effect);
    }
    public void AddEffect(StatusEffect effect, int duration)
    {
        if (PlayerScript.CurrentStatusEffects.ContainsKey(effect))
        {
            PlayerScript.CurrentStatusEffects[effect] += duration;
        }
        else
        {
            PlayerScript.CurrentStatusEffects.Add(effect, duration);
        }
    }
}
