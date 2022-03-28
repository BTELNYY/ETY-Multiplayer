using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class TickScript : NetworkBehaviour
{
    // Start is called before the first frame update
    [Header("Ticker Settings")]
    public float TickDelay = 0f;
    public float TickInterval = 0.04f;
    public bool OneObjectPerTick = false;
    //list of all gameobjects which should be ticked
    //thanks springcup
    public List<ITick> tickObjects = Globals.tickObjects;
    //private parts
    void Start()
    {
        //invokes as a repeating method with this method name, interval and delay set above
        if (!isClientOnly)
        {
            Debug.Log("Starting repeating method");
            StartCoroutine(TickUpdateCoroutine());
        }
    }
    private IEnumerator TickUpdateCoroutine()
    {
        yield return new WaitForSeconds(TickDelay); // wait before execution; we don't cache this because it only runs once

        WaitForSeconds tickIntervalWait = new WaitForSeconds(TickInterval); // cache the waiting time so you don't have to create a new one for each loop; this works because the value doesn't change (if it does change, feel free to modify the code to allow that)
        while (true)
        {
            foreach (ITick obj in tickObjects)
            {
                obj.Tick();
                if (OneObjectPerTick)
                {
                    yield return null;
                }
                // yield return null; // pause after ticking an object if you don't need them to be ticked in the same frame
            }
            yield return tickIntervalWait; // pause after ticking all objects
        }
    }
}
