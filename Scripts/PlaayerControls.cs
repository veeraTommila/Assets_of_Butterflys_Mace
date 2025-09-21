using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaayerControls : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody theRB;
    public float jumpForce = 5f;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;
    private Vector3 originalPosition;

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

    private Camera mainCam;
    private float maxRayDistance = 10f;
    private int floorMask;
    public Transform turret;
    public float turretTurningSpeed;

    public GameObject RestartMenu;

    public AudioClip damageSound;
    public AudioClip deathSound;

    private HashSet<GameObject> alreadyHitProjectiles = new HashSet<GameObject>();


    void Start()
    {
        PlayerDie = false;
        Enemy = GameObject.FindWithTag("Muzzle");

        controller = GetComponent<CharacterController>();
        theRB = GetComponent<Rigidbody>();
        originalPosition = transform.position;

        currentHealth = maximumHealth;
        healthBar.SetMaxHealth(maximumHealth);

        mainCam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
        CharacterAnimator.SetBool("PlayerDies", false);
    }

    void Update()
    {
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        CharacterAnimator.SetBool("isGrounded", controller.isGrounded);
        CharacterAnimator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));
/*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(0.5f);
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 0.5f);
        }*/
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        CharacterAnimator.SetBool("isGrounded", controller.isGrounded);

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            Vector3 targetDirection = hit.point - turret.position;
            targetDirection.y = 0f;
            Vector3 turningDirection = Vector3.RotateTowards(turret.forward, targetDirection, turretTurningSpeed * Time.deltaTime, 0f);
            turret.rotation = Quaternion.LookRotation(turningDirection);
        }
    }

    public void TakeDamage(float damage)
    {
        AudioSource.PlayClipAtPoint(damageSound, transform.position, 0.5f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            CharacterAnimator.SetBool("PlayerDies", true);
            GetComponent<PlaayerControls>().enabled = false;
            Debug.Log("Player is dead.");
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 0.5f);
            PlayerDie = true;
        }

        if (PlayerDie)
        {
            RestartMenu.gameObject.SetActive(true);
            Debug.Log("YOU DIED! Game over screen activated.");
            Player.transform.position = respawnpoint.position;
        }
    }

    public void Heal(float damage)
    {
        currentHealth += damage;
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Muzzle") && !alreadyHitProjectiles.Contains(other.gameObject))
        {
            alreadyHitProjectiles.Add(other.gameObject);
            TakeDamage(0.5f);
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 0.5f);

            // Optional: destroy the projectile after impact
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Muzzle"))
        {
            TakeDamage(0.5f);
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 0.5f);
        }
    }

}
