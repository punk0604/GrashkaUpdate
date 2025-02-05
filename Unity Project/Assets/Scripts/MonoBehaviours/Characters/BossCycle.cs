using System.Collections;

using UnityEngine;
 
public class BossCycle : MonoBehaviour

{

    // Boss Health Variables

    public int maxHealth = 100;

    private int currentHealth;

    private bool isRegenerating = false;

    private float lastHitTime;
 
    // Time between attacks

    int attackTime = 0;

    bool spawn = true;

    bool summon = false;

    CircleCollider2D circleCollider;
 
    public float pursuitSpeed;

    public float wanderSpeed;

    public float directionChangeInterval;

    protected float currentSpeed;

    public bool followPlayer;
 
    protected Coroutine moveCoroutine;

    protected Rigidbody2D rb2d;

    protected Animator animator;

    private SpawnPoint spawnZombie;
 
    protected Transform targetTransform = null;

    protected Vector3 endPosition;

    protected float currentAngle = 0;
 
    protected readonly string animationState = "AnimationState";
 
    protected enum CharStates

    {

        walkEast = 1,

        walkSouth = 2,

        walkWest = 3,

        walkNorth = 4,

        idleSouth = 5,

        attack = 6

    }
 
    protected virtual void Start()

    {

        animator = GetComponent<Animator>();

        rb2d = GetComponent<Rigidbody2D>();

        circleCollider = GetComponent<CircleCollider2D>();

        spawnZombie = GetComponent<SpawnPoint>();

        currentSpeed = wanderSpeed;

        currentHealth = maxHealth; // Initialize health

        StartCoroutine(WanderRoutine());

    }
 
    protected void FixedUpdate()

    {

        if (attackTime < 450)

        {

            attackTime++;

        }

        else

        {

            attackTime = 0;

            spawn = true;

        }
 
        // Start regenerating if no damage was taken for 5 seconds

        if (Time.time - lastHitTime >= 5f && !isRegenerating)

        {

            StartCoroutine(RegenerateHealth());

        }

    }
 
    public void TakeDamage(int damage)

    {

        currentHealth -= damage;

        lastHitTime = Time.time; // Update last hit time
 
        if (currentHealth <= 0)

        {

            Die();

        }

    }
 
    private IEnumerator RegenerateHealth()

    {

        isRegenerating = true;

        while (currentHealth < maxHealth)

        {

            currentHealth += 5; // Regenerate 5 health per second

            if (currentHealth > maxHealth) currentHealth = maxHealth;

            yield return new WaitForSeconds(1f);

        }

        isRegenerating = false;

    }
 
    private void Die()

    {

        Debug.Log("Boss defeated!");

        Destroy(gameObject); // Destroy the boss when dead

    }
 
    protected virtual void OnTriggerEnter2D(Collider2D collision)

    {

        if (collision.gameObject.CompareTag("Player") && followPlayer)

        {

            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;

        }
 
        if (moveCoroutine != null)

        {

            StopCoroutine(moveCoroutine);

        }
 
        moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

    }
 
    protected virtual void OnTriggerExit2D(Collider2D collision)

    {

        if (collision.gameObject.CompareTag("Player"))

        {

            animator.SetBool("isWalking", false);

            currentSpeed = wanderSpeed;
 
            if (moveCoroutine != null)

            {

                StopCoroutine(moveCoroutine);

            }
 
            targetTransform = null;

        }

    }
 
    protected void OnDrawGizmos()

    {

        if (circleCollider != null)

        {

            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);

        }

    }
 
    // Update is called once per frame

    protected void Update()

    {

        Debug.DrawLine(rb2d.position, endPosition, Color.red);

    }
 
    public IEnumerator WanderRoutine()

    {

        // Enemy should wander indefinitely

        while (true)

        {

            // Choose a new endpoint for the enemy to move toward

            ChooseNewEndpoint();
 
            // If enemy is already moving, stop it before moving in a new direciton

            if (moveCoroutine != null)

            {

                StopCoroutine(moveCoroutine);

            }

            // Start the new move routine

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
 
            yield return new WaitForSeconds(directionChangeInterval);

        }

    }
 
    protected void ChooseNewEndpoint()

    {

        // Choose a random value between 0 and 360 to represent a new direciton to travel toward

        currentAngle += UnityEngine.Random.Range(0, 360);
 
        // Effectively perform a mod operation so that currentAngle is always between 0 and 360

        currentAngle = Mathf.Repeat(currentAngle, 360);
 
        // Convert Angle to a Vector 3 and add result to endPosition

        endPosition += Vector3FromAngle(currentAngle);

    }
 
    // Takes an angle in degrees, converts it to radians, and return a directional vector

    protected Vector3 Vector3FromAngle(float inputAngleDegrees)

    {

        // Convert angle degrees to radians

        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
 
        // Create a normalized directional vector for the enemy direciton

        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);

    }
 
    protected virtual IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)

    {

        // Retrieve the rough distance remaining between teh current enemy position and the destination

        // Magnitude is a unity function to return the length of the vector

        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
 
        while (remainingDistance > float.Epsilon && !summon)

        {

            if (targetTransform != null)

            {

                // If targeTransform is set, then it's postion is the player's position

                // This moves the enemy toward the player instead of toward the original endPosition

                endPosition = targetTransform.position;

            }

            if (rigidBodyToMove != null && attackTime < 250)

            {

                // Set animation parameter so animator will change the anumations that's played

                animator.SetBool("isWalking", true);
 
                // Calculates the movement for a RigidBody2D

                Vector3 direction = (endPosition - transform.position).normalized;

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
 
                // Determine the direction to adjust the animation state

                // X AXIS

                if (direction.x > 0) animator.SetInteger(animationState, (int)CharStates.walkEast);

                else if (direction.x < 0) animator.SetInteger(animationState, (int)CharStates.walkWest);

                // Y AXIS

                else if (direction.y > 0) animator.SetInteger(animationState, (int)CharStates.walkNorth);

                else if (direction.y < 0) animator.SetInteger(animationState, (int)CharStates.walkSouth);

                else animator.SetInteger(animationState, (int)CharStates.idleSouth);
 
                // Move the RigidBody2D

                rb2d.MovePosition(newPosition);
 
                // Update the distance remaining

                remainingDistance = (transform.position - endPosition).sqrMagnitude;

            }

            else if (attackTime == 250 && spawn)

            {

                StartCoroutine(HandleZombieSpawn());

                //animator.SetInteger(animationState, (int)CharStates.attack);
 
            }

            else if (attackTime > 250)

            {

                animator.SetInteger(animationState, (int)CharStates.attack);

            }
 
 
            // Pause execution until the next Fixed Frame Update

            yield return new WaitForFixedUpdate();

        }
 
        animator.SetBool("isWalking", false);

        animator.SetInteger(animationState, (int)CharStates.idleSouth);

    }
 
    private IEnumerator HandleZombieSpawn()

    {

        spawnZombie.SpawnObject();

        yield return new WaitForSeconds(0.8f);

        spawnZombie.SpawnObject();

        spawn = false;

        yield return null;

    }

}