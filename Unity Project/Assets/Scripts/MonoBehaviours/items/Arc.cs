using System.Collections;
using UnityEngine;

public class Arc : MonoBehaviour
{
    // Reference to RigidBody2D
    [HideInInspector]public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Coroutine to move the gameobject; it will execute over several frames
    // Destination: the end position of the item
    // Duration: amount of time to move the gameobject from starting postion to destination
    public virtual IEnumerator TravelArc(Vector3 destination, float duration)
    {
        // Grab the current gameobject's position
        Vector3 startPosition = transform.position;

        float percentComplete = 0.0f;

        // check that percentComplete is less than 100%
        while(gameObject.activeInHierarchy && percentComplete < 1.0f)
        {
            // Time elapsed since the last frame, divided by the total desired duration = a percentage of the duration
            percentComplete += Time.deltaTime / duration;

            // To make the object move smoothly between two points at a constant speed, use Linear Interpolation
            // No matter where the AmmoObject is fired from, it will take the same amount of time to get there
            // Determines the distance to travel per frame
            // Returns a point between start and end based on the percentage
            rb.MovePosition(Vector2.Lerp(startPosition, destination, percentComplete));

            yield return null;
        }

        // Deactivate the object if it reaches its destination before hitting a Wall
        //gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
