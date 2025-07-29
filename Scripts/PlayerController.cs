using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// THIS IS THE PLAYER CONTROLLING CODE
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    //public float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;
    //Vector3 playerPrevPosition;
    //Vector3 playerCurPosition;
    private Vector3 originalPosition;
    private Vector3 currentPosition;

    public Animator CharacterAnimator; // An animator reference.
    public Transform pivot;
    public float rotateSpeed;
    private Rigidbody CharacterRigid;
    private GameObject Player;
    public GameObject Enemy;
    public Transform respawnpoint;
    int isFlyingHash;


    public float maximumHealth = 3;
    public float currentHealth;
    public HealthBar healthBar;
    Collider other;

    public GameObject playerModel;
    public UIController userInterface;
    public bool PlayerDie;

    private Camera mainCam;
    private float maxRayDistance = 10f;
    private int floorMask;
    public Transform turret;
    public float turretTurningSpeed;
    public Transform muzzle;
    public GameObject projectile;
    private float time;
    public float shootingCooldownTime;

    public GameObject RestartMenu;

    public AudioClip damageSound;
    public AudioSource deathSound;
    //public AudioSource audioSource;

    // public float movingSpeed;
    //public float turningSpeed;

    /*void Awake()
    {
        playerPrevPosition.y = transform.position.y;
        Debug.Log($"Player's Y position: {playerPrevPosition}");
    }*/

    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponent<Animator>();
        isFlyingHash = Animator.StringToHash("isFlying");

        PlayerDie = false;
        Enemy = GameObject.FindWithTag("Muzzle"); // Sets the variable to the GameObject that has a "Projectile" tag.       

        controller = GetComponent<CharacterController>();
        CharacterRigid = GetComponent<Rigidbody>();
        originalPosition = transform.position;

        currentHealth = maximumHealth;
        healthBar.SetMaxHealth(maximumHealth);

        mainCam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
        // A sanity check to make sure the player is not dead.
        CharacterAnimator.SetBool("PlayerDies", false);
    }

    void Update()
    {
        bool isFlying = CharacterAnimator.GetBool("isFlying");
        bool forwardPressed = Input.GetKey("w");

        if (!isFlying && forwardPressed)
        {
            CharacterAnimator.SetBool("isFlying", true);
        }

        if (isFlying && forwardPressed) {
            CharacterAnimator.SetBool("isFlying", false);
        }

        if (time <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject proje = Instantiate(projectile, muzzle.position, muzzle.rotation);
                proje.GetComponent<Projectile>().shooterTag = tag;
                time = shootingCooldownTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 1f);
        }



        /*if (PlayerDie == true)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }*/

    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
        CharacterRigid.MovePosition(pos);
        CharacterRigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

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
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
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
    } // End of Update function.


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
            //Play Game Over on screen.
            RestartMenu.gameObject.SetActive(true);
            //deathSound.Play();
            // Reload the whole scene and reset all the gameobjects.
            //Scene currentScene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(currentScene.name);
            // To set the respawn place of the player.
            Player.transform.position = respawnpoint.position;
        }
        RestartMenu.gameObject.SetActive(false);

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