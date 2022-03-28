using System.Collections;
using UnityEngine;

public class ItemYandereKnife : ItemBase
{
    public override void interact(PlayerScript ps)
    {
        base.interact(ps);
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