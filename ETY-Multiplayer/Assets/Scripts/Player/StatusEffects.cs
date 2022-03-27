using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusEffects : MonoBehaviour, ITick
{
    PlayerScript PlayerScript;
    int Ticks;
    void Start()
    {
        //fuck you, I dont care about bad game bullshit, this works in the end.
        PlayerScript = GetComponent<PlayerScript>();
    }
    public void Tick()
    {
        //25 times per second
        Ticks++; //tick updates
        EffectHandle(); //script handles effects
        if (Ticks == 25) //reset ticks
        {
            Ticks = 0;
        }
    }
    void EffectHandle()
    {
        //write code to handle each effect
        foreach (StatusEffect se in (StatusEffect[])Enum.GetValues(typeof(StatusEffect)))
        {
            switch (se)
            {
                case StatusEffect.None:
                    //do nothing
                    break;
                case StatusEffect.Burning:
                    EffectBurning();
                    break;
                case StatusEffect.Poison:
                    EffectPoison();
                    break;
                case StatusEffect.Regeneration:
                    EffectRegen();
                    break;
                case StatusEffect.God:
                    EffectGod();
                    break;
                case StatusEffect.Bleeding:
                    EffectBleeding();
                    break;
                default:
                    //do nothing, this will never happen becuase of the enum not being empty or having extra effects
                    break;
            }
        }
    }
    void EffectGod()
    {
        StatusEffect se = StatusEffect.God;
        if (PlayerScript.HasEffect(se))
        {
            if (PlayerScript.GetEffectDuration(se) > 0 && Ticks == 25)
            {
                PlayerScript.SetHealth(-1); //change line to do something different in a differnet method or otherwise.
                PlayerScript.ReduceEffectDuration(se, 1);
            }
            else
            {
                RemoveEffect(se);
            }
        }
    }
    void EffectBleeding()
    {
        StatusEffect se = StatusEffect.Bleeding;
        if (PlayerScript.HasEffect(se))
        {
            if (PlayerScript.GetEffectDuration(se) > 0 && Ticks == 25)
            {
                PlayerScript.SetLastDamage(PlayerScript.DeathTypes.Bleeding);
                PlayerScript.Damage(5f); //change line to do something different in a differnet method or otherwise.
                PlayerScript.ReduceEffectDuration(se, 1);
            }
            else
            {
                RemoveEffect(se);
            }
        }
    }
    void EffectPoison()
    {
        StatusEffect se = StatusEffect.Poison;
        if (PlayerScript.HasEffect(se))
        {
            if (PlayerScript.GetEffectDuration(se) > 0 && Ticks == 25)
            {
                PlayerScript.SetLastDamage(PlayerScript.DeathTypes.Poison);
                PlayerScript.Damage(7f); //change line to do something different in a differnet method or otherwise.
                PlayerScript.ReduceEffectDuration(se, 1);
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
        if (PlayerScript.HasEffect(se))
        {
            if (PlayerScript.GetEffectDuration(se) > 0 && Ticks == 25)
            {
                PlayerScript.Heal(2f, false); //change line to do something different in a differnet method or otherwise.
                PlayerScript.ReduceEffectDuration(se, 1);
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
        if (PlayerScript.HasEffect(se))
        {
            if (PlayerScript.GetEffectDuration(se) > 0 && Ticks == 25)
            {
                PlayerScript.SetLastDamage(PlayerScript.DeathTypes.Burning);
                PlayerScript.Damage(10f); //change line to do something different in a differnet method or otherwise.
                PlayerScript.ReduceEffectDuration(se, 1);
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
        Bleeding,
        Regeneration,
        God,
    }
    void RemoveEffect(StatusEffect effect)
    {
        PlayerScript.RemoveEffect(effect);
    }
}
