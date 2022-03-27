using System.Collections;
using UnityEngine;

public class ItemYandereKnife : ItemBase
{
    [System.NonSerialized]
    public new string Name = "Yandere Knife";
    public override void interact(PlayerScript ps)
    {
        base.interact(ps);
        Debug.Log("Interacting with " + Name);
        Destroy(gameObject);
    }
    public override void PrimaryUse(PlayerScript ps)
    {
        base.PrimaryUse(ps);
        Debug.Log("PrimaryUse");
    }
    public override void SecondaryUse(PlayerScript ps)
    {
        base.SecondaryUse(ps);
        Debug.Log("SecondaryUse");
    }
}