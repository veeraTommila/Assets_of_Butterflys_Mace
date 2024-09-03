using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// THIS IS THE ENEMY CONTROLLING CODE
public class EnemyControll : MonoBehaviour
{
    public Animator EnemyAnimator;
    public Rigidbody EnemyRigid;
    public GameObject Player;
    public PlayerController playerHealth;
    public float damage ;

    public float EnemyWalkingSpeed = 5; // How fast the enemy moves.
    public float EnemyRunningSpeed = 10; // How fast enemy runs.
    public float EnemyHittingSpeed = 6; // Melee speed.
    public float EnemyDistance = 10000; // A default distance for the EnemyDistance.
    public float EnemyAttackDistance = 300; // This is the distance at which the enemy slows to walk and starts hitting.
    public float EnemyHittingDistance = 5; // This is the distance at which the enemy starts hitting.
    public float EnemyHittingRate = 2; // How often the enemy hits.

    public bool PlayerSighted; // A true/false variable about whether or not the player has been found.
    public bool RunAtPlayer; // This tells the program whether or not to have the enemy running at player.
    public bool EnemyShoot;
    public bool HittingAttack; // The melee attack. Is the enemy attacking with a hitting movement or not.
    public bool EnemyDying; // is the enemy dying or not. This will call the dying animation and then destroy the enemy.
    public float EnemyDeathDuration = 10; // How long must we wait until the object dissapears in the scene.

    public float HittingTimer; // The melee timer. This timer works with the rate of hitting attack.   

    public Transform turret;
    public float turretTurningSpeed;
    public Transform muzzleBone;
    public GameObject projectile;
    private float time;
    public float shootingCooldownTime;
    private float damageTimer = 0;
    public float damageDelay = 1;
    public GameObject[] muzzleBones;

    public Transform respawnpoint;
    private Ray ray;
    private RaycastHit raycastHit;
    private float sphereCastRadius = 12.0f;
    private float maxRayDistance = 10f;
    public LayerMask layersToHit;
    /*
    public float maximumHealth = 3;
    public float currentHealth;
    public HealthBar healthBar;
    */

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player"); // Sets the variable to the GameObject that has a "Player" tag.
        EnemyAnimator = GetComponent<Animator>(); // Looks for the Animator so that we can call the animations from the controller.
        EnemyRigid = GetComponent<Rigidbody>();
        HittingTimer = Time.time + EnemyHittingRate;
        PlayerSighted = false;
        RunAtPlayer = false;
        EnemyShoot = false;
        HittingAttack = false;
        EnemyDying = false;

        ray = new Ray(transform.position, transform.forward);
        CheckForColliders();
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;
        EnemyDistance = Vector3.Distance(transform.position, Player.transform.position);

        EnemyAnimator.SetBool("EnemyRunOn", RunAtPlayer); // Sets the bool that was set up in the Animation Controller equal to bool in the program.
        EnemyAnimator.SetBool("EnemyWalkOn", EnemyShoot); // Sets the bool that was set up in the Animation Controller equal to bool in the program.
        EnemyAnimator.SetBool("EnemyMeleeOn", HittingAttack); // Sets the bool that was set up in the Animation Controller equal to bool in the program.
        EnemyAnimator.SetBool("EnemyDyingOn", EnemyDying); // Sets the bool that was set up in the Animation Controller equal to bool in the program.

        /*muzzleBones = GameObject.FindGameObjectsWithTag("Muzzle");

        for (int i = 0; i < muzzleBones.Length; i++)
        {
            Transform muzzleBone = muzzleBones[i].GetComponent<Transform>();
            if (muzzleBone != null)
            {
                Debug.Log("Ou!");
            }
        }*/


        if(EnemyDying == true)
        {
            enemyDead();
        }
        
        if(time <= 0)
        {
            if (PlayerSighted == true)
            {
                playerFound();

                // Set the position and rotation of the enemy's projectile object to selected muzzleBone bone.
                GameObject proj = Instantiate(projectile, muzzleBone.position, muzzleBone.rotation);
                proj.GetComponent<Projectile>().shooterTag = tag;
                time = shootingCooldownTime;
            }            

        }

        if(Physics.SphereCast(ray, sphereCastRadius, out raycastHit))
        {
            GameObject sphereGameObject = raycastHit.transform.gameObject;
        }
        
    } // End of the Update function.

    
    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Reload the whole scene and reset all the gameobjects.
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            // To set the respawn place of the player.
            //Player.transform.position = respawnpoint.position;
        }
    }*/
        
   void OnTriggerEnter(Collider other) 
    {
        // This tests to see if an object enters the collider and whether or not it's the Player.
        if (other.CompareTag("Player")) // If the object is the player, it plays a sound to warn the player they've been sensed.
        {
            //AudioSource EnemyStartUp = GetComponent<AudioSource>();
            //EnemyStartUp.Play();
            Debug.Log("Enemy starts up.");

            /*var healthComponent = other.GetComponent<PlayerController>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(1);
                Debug.Log("Take damage.");
            }
            if (healthComponent = null)
            {
                healthComponent.Heal(1);
                Debug.Log("Heal from damage.");
            }*/
            
            //playerHealth.TakeDamage(damage);  //When this is active, the restart menu shows up.            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider that has collided with the enemy is the player 
        if (collision.collider.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // As long as the player is inside the collider, this variable stays true.
        if (other.CompareTag("Player")) // Is the object Player or not.
        {
            PlayerSighted = true; // This variable is being looked for in the Update function, which is constantly running.
            Debug.Log("PlayerSighted is true.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // If the Player leaves the sphere, this resets all the variables.
        {
            PlayerSighted = false;
            RunAtPlayer = false;
            HittingAttack = false;
            EnemyShoot = false;
            Debug.Log("On trigger Exit.");
        }
    } // Now the enemy won't follow the player anymore.

    void playerFound()
    {
        Debug.Log("Player found!");

        Vector3 lookAtPosition = Player.transform.position; // Sets the variable as the Player position.
        lookAtPosition.y = transform.position.y; // Sets the direction only in Y-axis to eliminate the up and down.
        transform.LookAt(lookAtPosition); // Points the enemy at the Player character. The swatter will turn towards the butterfly

        if (EnemyDistance >= EnemyAttackDistance) // The enemy runs at player if the player is outside the range.
        {
            transform.position += transform.forward * EnemyRunningSpeed * Time.deltaTime;
            // Sets the variables to play the correct animation play.
            RunAtPlayer = true; 
            HittingAttack = false;
            EnemyShoot = false;
        }

        if ((EnemyDistance < EnemyAttackDistance) & (EnemyDistance > EnemyHittingDistance)) 
        {
            Debug.Log("I am firing you!");
            // If the player is in the attacking range, the enemy fires.
            transform.position += transform.forward * EnemyWalkingSpeed * Time.deltaTime; // Enemy slows down to walk and starts firing.
            // Sets the variables to play the correct animation play.
            RunAtPlayer = false;
            HittingAttack = false;
            EnemyShoot = true;
        }
            

        if (EnemyDistance <= EnemyHittingDistance)
        {
            //AudioSource EnemyStartUp = GetComponent<AudioSource>();
            //EnemyStartUp.Play();
            Debug.Log("I am hitting you!");
            // If the enemy is to close to fire, it rushes in and hits the player. The swatter should hit the butterfly.
            transform.position += transform.forward * EnemyHittingSpeed * Time.deltaTime;
            RunAtPlayer = true;
            //HittingAttack = true;

            if (Time.deltaTime > HittingTimer)
            {
                Debug.Log("I am hitting you 2!");
                // Sets the variables to play the correct animation play.
                RunAtPlayer = false; 
                HittingAttack = true;
                EnemyShoot = false;
                HittingTimer = Time.deltaTime + EnemyHittingRate;
            }
            else
            {
                HittingAttack = false; // Just in case, this makes sure the bool is set to false if the timer is not ready.
            }
        }
    } // End of teh playerFound function.

    void CheckForColliders()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance,layersToHit))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit!");
            playerHealth.TakeDamage(damage);
        }
    }

    /*
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }*/

    void enemyDead()
    {
        Debug.Log("You go, I die!");
        EnemyAnimator.SetBool("EnemyDyingOn", EnemyDying); // Sets the bool that was set up in the Animation Controller equal to bool in the program.
        Destroy(this.gameObject, EnemyDeathDuration); // Destroys the spawned projectile at the specified time.
    } // End of the enemy function
} // End of the program.
