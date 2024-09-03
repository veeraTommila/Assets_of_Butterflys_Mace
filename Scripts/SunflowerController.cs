using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerController : MonoBehaviour
{
    private Rigidbody FlowerRigid;
    public GameObject Player;
    public bool PlayerIsHere;
    [SerializeField]
    GameObject WonMenu;
    public AudioClip successSound;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player"); // Sets the variable to the GameObject that has a "Player" tag.        
        FlowerRigid = GetComponent<Rigidbody>();
        PlayerIsHere = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // This tests to see if an object enters the collider and whether or not it's the Player.
        if (other.CompareTag("Player")) // If the object is the player, it plays a sound to warn the player they've been sensed.
        {
            //AudioSource EnemyStartUp = GetComponent<AudioSource>();
            //EnemyStartUp.Play();
            Debug.Log("You won the level!");

            PlayerIsHere = true;
            if (PlayerIsHere == true)
            {
                WonMenu.gameObject.SetActive(true);
                AudioSource.PlayClipAtPoint(successSound, transform.position, 1f);
            }
        }
    }
}
