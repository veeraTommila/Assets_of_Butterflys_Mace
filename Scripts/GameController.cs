using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SpawnerCode spawner;
    
    public float score;
    public float scorePerPoint;
    public int lives;
    public int enemyStartAmount;
    public int enemyMaxAmount;

    private int currentLives;
    private int enemyCurAmount;

    private GameObject Player;
    public static GameController instance;
    public UIController userInterface;

    public Animator CharacterAnimator;
    public bool PlayerDie;
    private HealthBar playerHealth;
    public UIController GameOverScreen;

    public void GameOver()
    {
        GameOverScreen.SetUP(lives);
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerDie = false;
        for (int i = 0; i < enemyStartAmount; i++)
        {
            spawner.SpawnEnemy();
        }

        //Player = spawner.SpawnPlayer();
        playerHealth = Player.GetComponent<HealthBar>();

        score = 0f;
        currentLives = lives;
        enemyCurAmount = enemyStartAmount;

        userInterface.SetLives(currentLives, lives);

        
    } // End of Start function.

    // Update is called once per frame
    void Update()
    {
        if (currentLives > 0)
        {

            // Check if the player object is null (i.e., player is dead)
            if (Player == null)
            {
                // Show the restart menu
                userInterface.ShowRestartMenu();

                // Check for restart input
                if (Input.GetButtonDown("Restart"))
                {
                    // Respawn the player
                    //Player = spawner.SpawnPlayer();
                    currentLives--;
                    userInterface.SetLives(currentLives, lives);
                    userInterface.HideRestartMenu();
                }
            }
        }
    }


    public void EnemiesDestroyed()
    {
        spawner.SpawnEnemy();
        score += scorePerPoint;

        if(enemyCurAmount < enemyMaxAmount)
        {
            spawner.SpawnEnemy();
            enemyCurAmount++;
        }
    }

    public void AddScore()
    {
        score += scorePerPoint;
    }

    public void SetHealth(int currentHealth, int maximumHealth)
    {
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
        userInterface.SetHealth(currentHealth, maximumHealth);
    }
}
