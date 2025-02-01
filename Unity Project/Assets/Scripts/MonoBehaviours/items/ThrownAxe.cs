using Unity.Burst.Intrinsics;
using UnityEngine;

public class ThrownAxe : MonoBehaviour
{
    // Amount of damage the ammunition will inflict on an enemy
    public int damageInflicted;

    // Whether the Ammo can damage multiple enemies before being deactivated
    public bool piercing;

    // Amount of trigger entries detected since thrown attack/ammo being active
    private int collisions = 0;

    // Called when another object enters the trigger collider attached to the ammo gameobject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if thrown attack/ammo is still active and only the first collision is considered
        if (gameObject.activeInHierarchy && collisions == 0)
        {
            // Thrown attack/ammo hits Enemy
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Retrieve the player script from the enemy object
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                // Start the damage coroutine; 0.0f will inflict a one-time damage
                StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f, 2));


                // Deactivate Ammo and rearm Grashka if it can't pierce enemies
                if (!piercing)
                {
                    gameObject.SetActive(false);
                    Axe.Instance.armed = true;
                }

                // Register collision
                collisions++;
            }

            // Thrown attack/ammo hits Wall
            if (collision.gameObject.CompareTag("Wall"))
            {
                // Deactivate thrown attack/ammo, rearm Grashka (so rearm is done without travelling the Arc/AxeArc first)
                gameObject.SetActive(false);
                Axe.Instance.armed = true;

                // Register collision
                collisions++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collisions == 1)
        {
            collisions = 0;
        }
    }
}
