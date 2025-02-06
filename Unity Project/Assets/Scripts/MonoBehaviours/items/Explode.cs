using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float blastRadius; // Radius of damging blast in units
    public int explosionDamageInflicted; // Specifically damage done to characters in the blast radius
    private Animator animator; // Explosion's animator

    private void Start()
    {
        // Scale explosion and triggers to be blastRadius big
        gameObject.transform.localScale = blastRadius * Vector3.one;

        // Initailize Animator
        animator = gameObject.GetComponent<Animator>();
        
    }

    private void Update()
    {
        // If animation is finished, set explosion inactive
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Retrieve the Enemy script from the enemy object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Start the damage coroutine; 0.0f will inflict a one-time damage
            StartCoroutine(enemy.DamageCharacter(explosionDamageInflicted, 0.0f));
        }
    }

}
