using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    [Header("Collider Settings")]
    public string GameObjectName;
    public string GameObjectTag;
    public bool DetectCollisions = true;
    //protected prevents people from accessing this method, this is needed to prevent things from copying and duplicating
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == GameObjectName && DetectCollisions)
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            OnNameCollider(collision);
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == GameObjectTag && DetectCollisions)
        {
            //If the GameObject has the same tag as specified, output this message in the console
            OnTagCollider(collision);
        }
    }
    
    //these public classes have to be overriden in order to do things
    void OnTagCollider(Collision col)
    {
        if (!DetectCollisions)
        {
            return;
        }
        PlayerScript player = col.gameObject.GetComponent<PlayerScript>();
        player.AddEffect(StatusEffects.StatusEffect.Poison, 10);
    }
    void OnNameCollider(Collision col)
    {
        if (!DetectCollisions)
        {
            return;
        }

    }
}
