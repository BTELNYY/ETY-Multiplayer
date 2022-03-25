using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class Globals
{
    //handles all global vars
    public static List<ITick> tickObjects = new List<ITick>();

    [Obsolete("AddTickObject is deprecated, please use AddITick instead.")]
    public static void AddTickObject(GameObject obj)
    {
        //wouldnt this cause issues with multiple objects named the same thing?
        //no, since the properties are different, so this should work with mirror aswell.
        ITick tickthing = obj.GetComponent<ITick>();
        if (!tickObjects.Contains(tickthing))
        {
            tickObjects.Add(tickthing);
        }
    }
    public static void AddITick(ITick obj)
    {
        if (!tickObjects.Contains(obj))
        {
            tickObjects.Add(obj);
        }
    }
    [Obsolete("RemoveTickObject is deprecated, please use RemoveITick instead.")]
    public static void RemoveTickObject(GameObject obj)
    {
        ITick tickthing = obj.GetComponent<ITick>();
        if (tickObjects.Contains(tickthing))
        {
            tickObjects.Remove(tickthing);
        }
    }
    public static void RemoveITick(ITick obj)
    {
        if (tickObjects.Contains(obj))
        {
            tickObjects.Remove(obj);
        }
    }
}