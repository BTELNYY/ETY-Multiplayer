using System.Collections;
using UnityEngine;

public class ItemNoneScript : ItemBase
{
    //deprecated class
    [System.NonSerialized]
    public new string Name = "None";
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