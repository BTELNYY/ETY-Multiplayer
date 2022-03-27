using System.Collections;
using UnityEngine;


public class DeathTrigger : MonoBehaviour
{
    [Header("Collider Settings")]
    public string GameObjectTag = "Player";
    public bool DetectCollisions = true;
    public void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag(GameObjectTag) && DetectCollisions)
        {
            OnTagCollider(collision);
        }
    }
    public void OnTagCollider(Collision col)
    {
        if (!DetectCollisions)
        {
            return;
        }
        //gets the players controller script, and kills them
        PlayerScript ps = col.gameObject.GetComponent<PlayerScript>();
        //if they have the God status effect, ignore them
        if (ps.HasEffect(StatusEffects.StatusEffect.God))
        {
            return;
        }
        else
        {
            //kills the player
            ps.Suicide();
        }
    }
}