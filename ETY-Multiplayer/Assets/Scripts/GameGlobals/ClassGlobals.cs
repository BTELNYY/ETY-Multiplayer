using System.Collections;
using UnityEngine;
public class ClassGlobals : MonoBehaviour
{
    public GameObject DebugPlayer;

    public static ClassGlobals Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}