using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    GameObject RestartMenu;
    public float maxHealth = 3;
    private float currentHealth;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void ReduceHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            anim.SetBool("PlayerDies", true);
            GetComponent<PlayerController>().enabled = false;
            RestartMenu.gameObject.SetActive(true);
        }
    }
}
