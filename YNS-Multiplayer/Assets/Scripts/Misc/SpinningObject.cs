using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    [Header("Object Settings")]
    public Transform Object;
    [Header("Spin Settings")]
    public float TimeScale;
    public float SpinX;
    public float SpinY;
    public float SpinZ;
    [Header("Float Settings")]
    public bool Float;
    public float FloatDistance;
    // Update is called once per frame
    void Start()
    {
        //moves the object to the float entered by the code
        if (Float)
        {
            Object.position = new Vector3(Object.position.x, Object.position.y + FloatDistance, Object.position.z);
        }
    }
    void Update()
    {
        //spins the object slightly per frame depending on settings
        Object.Rotate(new Vector3(SpinX, SpinY, SpinZ) * TimeScale * Time.deltaTime);
    }
}