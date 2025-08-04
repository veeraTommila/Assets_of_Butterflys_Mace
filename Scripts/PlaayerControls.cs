using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaayerControls : MonoBehaviour
{
    public float moveSpeed;   //Tätä muuttujaa moveSpeed voidaan editoida Unityssä, private floatia ei voida.
    private Rigidbody theRB;   //Otetaan Rigidbody käyttöön.
    private float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;
    private Vector3 originalPosition;
    private Vector3 currentPosition;

    public Animator CharacterAnimator;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject Player;
    public GameObject Enemy;
    public Transform respawnpoint;
    public bool PlayerDie;

    public float maximumHealth = 3;
    public float currentHealth;
    public HealthBar healthBar;
    Collider other;

    private Camera mainCam;
    private float maxRayDistance = 10f;
    private int floorMask;
    private Transform turret;
    private float turretTurningSpeed;
    private Transform muzzle;
    //public GameObject projectile;
    private float time;
    private float shootingCooldownTime;

    public GameObject RestartMenu;

    public AudioClip damageSound;
    public AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        PlayerDie = false;
        Enemy = GameObject.FindWithTag("Muzzle"); // Sets the variable to the GameObject that has a "Projectile" tag.       

        controller = GetComponent<CharacterController>();
        theRB = GetComponent<Rigidbody>();
        originalPosition = transform.position;

        currentHealth = maximumHealth;
        healthBar.SetMaxHealth(maximumHealth);

        mainCam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
        // A sanity check to make sure the player is not dead.
        CharacterAnimator.SetBool("PlayerDies", false);
    }

    // Update is called once per frame
    void Update()
    {
        //Pelinappulan liikkeen kontrollointi on helpompaa.
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;


        // Has the jump key been pressed in Unity?
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                // Let's change the value of the Y-axis.
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * Time.deltaTime * gravityScale);

        // Apply moveDirection to Character Controller.
        controller.Move(moveDirection * Time.deltaTime);

        // The game character rotates according to the direction the camera is looking.
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        CharacterAnimator.SetBool("isGrounded", controller.isGrounded);
        CharacterAnimator.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 1f);
        }
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
        //theRB.MovePosition(pos);
        //theRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        // The movement of the player object will be easier.
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        // Let us apply the moveDirection to controller.
        controller.Move(moveDirection * Time.deltaTime);

        // The player object turns to the camera's watching angle.
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        CharacterAnimator.SetBool("isGrounded", controller.isGrounded);
        //CharacterAnimator.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        //The ray.
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        //If the raycast with ray hit's something, this happens with max distance.
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            Vector3 targetDirection = hit.point - turret.position;
            targetDirection.y = 0f;
            Vector3 turningDirection = Vector3.RotateTowards(turret.forward, targetDirection, turretTurningSpeed * Time.deltaTime, 0f);
            turret.rotation = Quaternion.LookRotation(turningDirection);
        }
    } // End of FixedUpdate function.

    

    public void TakeDamage(float damage)
    {
        AudioSource.PlayClipAtPoint(damageSound, transform.position, 1f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Character dies.
            //The death animation is played.
            CharacterAnimator.SetBool("PlayerDies", true);
            // The player won't move.
            GetComponent<PlayerController>().enabled = false;
            Debug.Log("Player is dead.");
            //AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
            //userInterface.ShowRestartMenu();
            PlayerDie = true;
        }

        if (PlayerDie == true)
        {
            // Play Game Over on screen.
            RestartMenu.gameObject.SetActive(true);
            Debug.Log("YOU DIED! This will tell that the game over screen will be played now.");
            //deathSound.Play();
            // Reload the whole scene and reset all the gameobjects.
            //Scene currentScene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(currentScene.name);
            // To set the respawn place of the player.
            Player.transform.position = respawnpoint.position;
        }
        //RestartMenu.gameObject.SetActive(false);

    }

    public void Heal(float damage)
    {
        currentHealth += damage;

        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
    }
}
