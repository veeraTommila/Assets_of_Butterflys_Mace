using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    GameObject RestartMenu;

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public float maximumHealth;
    private float currentHealth;

    Animator anim;

    public Transform respawnpoint;
    private GameObject Player;

    public GameObject explosion;

    public float damageFlashTime = 1f;
    public Color damageColor = Color.red;
    private Color originalColor;
    private float time;

    private MeshRenderer[] meshrenderers;

    public AudioClip deathSound;

    void Start()
    {
        currentHealth = maximumHealth;
        anim = GetComponent<Animator>();
        meshrenderers = GetComponentsInChildren<MeshRenderer>();
        originalColor = meshrenderers[0].material.color;
        
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            anim.SetBool("PlayerDies", true);
            GetComponent<PlayerController>().enabled = false;
            RestartMenu.gameObject.SetActive(true);
        }

        if (currentHealth < maximumHealth)
        {
            currentHealth++;
        }
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;        

        if (currentHealth <= 0)
        {
            anim.SetBool("PlayerDies", true);
            GetComponent<PlayerController>().enabled = false;
            RestartMenu.gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }
    }

    public void ReduceHealth(float damage)
    {
        StartCoroutine(DamageFlash());
        currentHealth -= damage;
        SetHealth(currentHealth);

        // If the player has no life points the player will be destroyed.
        if (currentHealth <= 0)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            //Destroy(gameObject);
            Debug.Log("Reducing health.");
            //Player.transform.position = respawnpoint.position;
        }
    }

    // A coroutine function DamageFlash.
    private IEnumerator DamageFlash()
    {
        time = damageFlashTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            foreach (MeshRenderer meshrederer in meshrenderers)
            {
                meshrederer.material.color = damageColor;
            }
            yield return null;
        }
        foreach (MeshRenderer meshrederer in meshrenderers)
        {
            meshrederer.material.color = originalColor;
        }
    }

    
}
