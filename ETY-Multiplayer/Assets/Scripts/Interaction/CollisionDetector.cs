using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Header("Collider Settings")]
    public string GameObjectName;
    public string GameObjectTag;
    public bool DetectCollisions = true;
    //protected prevents people from accessing this method, this is needed to prevent things from copying and duplicating
    public void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == GameObjectName && DetectCollisions)
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            OnNameCollider();
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag(GameObjectTag) && DetectCollisions)
        {
            //If the GameObject has the same tag as specified, output this message in the console
            OnTagCollider();
        }
    }
    //these public classes have to be overriden in order to do things
    public void OnTagCollider()
    {
        if(!DetectCollisions)
        {
            return;
        }

    }
    public void OnNameCollider()
    {
        if (!DetectCollisions)
        {
            return;
        }

    }
}
