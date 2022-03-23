using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class Globals
{
    //handles all global vars
    public static List<GameObject> tickObjects = new List<GameObject>();

    public static void AddTickObject(GameObject obj)
    {
        //wouldnt this cause issues with multiple objects named the same thing?
        //no, since the properties are different, so this should work with mirror aswell.
        if (!tickObjects.Contains(obj))
        {
            tickObjects.Add(obj);
        }
    }
    public static void RemoveTickObject(GameObject obj)
    {
        if (tickObjects.Contains(obj))
        {
            tickObjects.Remove(obj);
        }
    }
}