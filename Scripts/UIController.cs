using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText;
    public Text livesText;
    public Text endScore;

    public GameObject startMenu;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject restartMenu;
    public GameObject endMenu;
    public GameObject infoMenu;
    public GameObject dieMenu;
    public GameObject volumeMenu;

    public float maximumHealth = 3f;
    public float currentHealth;
    Collider other;
    private HealthBar playerHealth;
    private GameObject player;
    public bool isDead;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        ShowStartMenu();
        ShowMainMenu();
        playerHealth = player.GetComponent<HealthBar>();
        
    } // End oft Start function.

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
        
    } // End oft Update function.

    // Public functions to change UI-texts.
    public void PlayGame()
    {
        //SceneManager.LoadSceneAsync(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("The button is working");
    }
    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void SetLives(int currentLives, int maxLives)
    {
        livesText.text = "Lives: " + currentLives + "/" + maxLives;
    }

    public void TogglePause()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);           
            Time.timeScale = 1f;
        }
        else
        {
            pauseMenu.SetActive(true);            
            Time.timeScale = 0f;    // Every object stops moving.
        }
    }

    public void InfoPause()
    {
        if (infoMenu.activeInHierarchy)
        {
            infoMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            infoMenu.SetActive(true);
            Time.timeScale = 0f;    // Every object stops moving.
        }
    }

    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
    }

    public void HideStartMenu()
    {
        startMenu.SetActive(false);
    }
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);        
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void ShowRestartMenu()
    {        
        restartMenu.SetActive(true);
        //AudioSource.PlayClipAtPoint(deathSound, transform.position, 3f);        
    }

    public void ShowInfoMenu()
    {
        infoMenu.SetActive(true);
        InfoPause();
    }

    public void HideInfoMenu()
    {
        infoMenu.SetActive(false);
    }

    public void ShowVolumeMenu()
    { 
        volumeMenu.SetActive(true);
    }

    public void HideVolumeMenu()
    {
        volumeMenu.SetActive(false);
    }

    public void SetUP(int health)
    {
        gameObject.SetActive(true);
        healthText.text = health.ToString() + " lives";
    }

    public void HideRestartMenu()
    {
        restartMenu.SetActive(false);
    }

    public void ShowEndMenu(int score)
    {
        endMenu.SetActive(true);
    }    

    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadSceneAsync("Level2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadSceneAsync("Level3");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
