using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlidingDoor : InteractableObject
{
    [Header("Door Setup")]
    public Transform DoorModel;
    public float Speed;
    public float MoveX;
    public bool KeepCurrentX;
    public float MoveY;
    public bool KeepCurrentY;
    public float MoveZ;
    public bool KeepCurrentZ;
    public bool MoveBackAfterUse = true;
    Vector3 OriginalPosition;
    Vector3 MovePosition;
    bool startMove = false;
    bool doorInPosition;
    public override void interact()
    {
        startMove = !startMove;
    }
    void Start()
    {
        OriginalPosition = DoorModel.position;
        if (KeepCurrentX)
        {
            MoveX = OriginalPosition.x;
        }
        if (KeepCurrentY)
        {
            MoveY = OriginalPosition.y;
        }
        if (KeepCurrentZ)
        {
            MoveZ = OriginalPosition.z;
        }
        MovePosition = new Vector3(MoveX, MoveY, MoveZ);
        startMove = false;
    }
    void Update()
    {
        if (doorInPosition == true && startMove == false)
        {
            MoveBack();
        }
        else
        {
            doorInPosition = Move();
        }
    }
    void MoveBack()
    {
        if (DoorModel.position != OriginalPosition && MoveBackAfterUse)
        {
            Vector3 newPos = Vector3.MoveTowards(DoorModel.position, OriginalPosition, Speed * Time.deltaTime);
            DoorModel.position = newPos;
        }
        if(DoorModel.position == OriginalPosition)
        {
            startMove = false;
        }
    }
    bool Move()
    {
        if (DoorModel.position != MovePosition && startMove)
        {
            Vector3 newPos = Vector3.MoveTowards(DoorModel.position, MovePosition, Speed * Time.deltaTime);
            DoorModel.position = newPos;
            return false;
        }
        else
        {
            return true;
        }
    }
}
