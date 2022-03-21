using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TickScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Ticker Settings")]
    public float TickDelay = 0f;
    public float TickInterval = 0.04f;
    //list of all gameobjects which should be ticked
    public List<GameObject> tickObjects = new List<GameObject>();
    void Start()
    {
        //invokes as a repeating method with this method name, interval and delay set above
        InvokeRepeating("TickUpdate", TickDelay, TickInterval);
    }
    void TickUpdate()
    {
        foreach (GameObject obj in tickObjects)
        {
            ITick tick = obj.GetComponent<ITick>();
            if (tick == null)
            {
                //if ITick is not found
                Debug.LogError(obj.ToString() + " does not implement the ITick interface even though it is registered as a tick object.");
            }
            else
            {
                //tick the object
                tick.Tick();
            }
        }
    }
    public void addObject(GameObject obj)
    {
        if (tickObjects.Contains(obj))
        {
            return;
        }
        else
        {
            tickObjects.Add(obj);
        }
    }
}
