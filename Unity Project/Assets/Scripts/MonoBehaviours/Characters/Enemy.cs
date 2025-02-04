using UnityEngine;
using System.Collections;


public class Enemy : Character
{
    private float hitPoints;

    // Amount of damage the enemy will inflict when it runs into the player
    public int damageStrength;

    // Reference to a running coroutine
    Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }

    // Give Enemy its starting hp
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    // Apply damage to Enemy, objectimpact = 1: player attack collide, = 2: player ammo collide, interval yet to be implemented
    public override IEnumerator DamageCharacter(int damage, float interval, float objectImpact)
    {
        // inflict damage
        hitPoints -= damage;

        // enemy is dead; kill off game object and exit loop
        if (hitPoints <= 0)
        {
            KillCharacter();
        }

        if (interval > 0)
        {
            // wait a specified amount of seconds and inflict more damage
            yield return new WaitForSeconds (interval);

            // inflict damage
            hitPoints -= damage;
        }
        
    }

    // Called by the Unity engine whenever the current enemy object's Collider2D makes contact with another object's Collider2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // See if the enemey has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get a reference to the colliding player object
            Player player = collision.gameObject.GetComponent<Player>();

            // If coroutine is not currently executing
            // start the coroutien to inflict damage to the player every 1 second
            if (damageCoroutine == null)
            {
                damageCoroutine ??= StartCoroutine(player.DamageCharacter(damageStrength, 0.0f, 1));
            }
            else if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    // Called by the Unity engine whenever the current enemy object stops touching another object's Collider2d
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    // See if the enemy has just stopped colliding with the player
    //if (collision.gameObject.CompareTag("Player"))
    //{
    // If coroutine is currrently executing
    //if (damageCoroutine != null)
    //{
        //StopCoroutine(damageCoroutine);
        //damageCoroutine = null;
    //}
    //}
    //}

    public override void KillCharacter()
    {
        // Call KillCharacter in parent(Character) class, which will destroy thec player game object
        base.KillCharacter();
    }
}
