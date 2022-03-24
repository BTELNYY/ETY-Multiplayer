using System.Collections;
using UnityEngine;

public class ItemNoneScript : ItemBase
{
    [System.NonSerialized]
    public new string Name = "None";
    public override void PrimaryUse(GameObject obj)
    {
        base.PrimaryUse(obj);
        Debug.Log("PrimaryUse");
    }
    public override void SecondaryUse(GameObject obj)
    {
        base.SecondaryUse(obj);
        Debug.Log("SecondaryUse");
    }
}