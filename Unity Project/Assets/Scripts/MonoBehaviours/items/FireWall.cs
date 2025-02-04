using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{

    // Damage dealt upon contact then again after interval seconds
    public int burningDamageInflicted;

    // Seconds until second damage
    public float interval;

    // Seconds firewall lasts
    public float lifetime;

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Inflict damage, then again interval seconds later
            Enemy enemyScript = collision.GetComponent<Enemy>();
            StartCoroutine(enemyScript.DamageCharacter(burningDamageInflicted, interval, 1));
        }
    }
}
