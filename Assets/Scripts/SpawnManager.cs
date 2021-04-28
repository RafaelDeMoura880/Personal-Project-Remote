using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject powerUp;
    private PlayerController playerController;
    public GameObject titleScreen;
    public Button startButton;
    public Button restartButton;
    public GameObject winText;
    private AudioSource menuMusic;
    private AudioSource gameSounds;
    private AudioSource gameSoundtrack;  //"soundtrack" when the game starts
    //private AudioSource gameWonSound;
    public AudioClip powerupPickUp;
    public AudioClip gameoverSound;
    public AudioClip powerupSpawnSound;
    public AudioClip enemyDestroySound;
    public AudioClip powerupBounceSound;
    public AudioClip gameWonSound;
    public float spawnRangeX = 49f;
    public float spawnRangeZ = 49f;
    public bool wave1;
    public bool wave2;
    public bool wave3;
    public bool killedHeavy;
    public bool killedMedium;
    public bool killedLight;
    public bool isGameOver;
    public bool oneShot;

    void Start()
    {
        //gameWonSound = GameObject.Find("GameWonSound").GetComponent<AudioSource>();
        gameSoundtrack = GameObject.Find("Soundtrack").GetComponent<AudioSource>();
        gameSounds = GetComponent<AudioSource>();
        menuMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        startButton.onClick.AddListener(StartGame);
    }

    // Start Game function
    public void StartGame()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnPowerUp();
        SpawnEnemyHeavy();
        wave1 = true;
        wave2 = false;
        wave3 = false;
        titleScreen.gameObject.SetActive(false);
        menuMusic.Stop();
        gameSoundtrack.PlayDelayed(.5f);
        isGameOver = false;
        oneShot = false;
    }

    //Enemy types, waves and spawns
    void Update()
    {
        if(wave1 == true && killedHeavy == true)
        {
            wave2 = true;
            SpawnEnemyHeavy();
            SpawnEnemyMedium();
            wave1 = false;
        }
        if(wave2 == true && killedHeavy == true && killedMedium == true)
        {
            wave3 = true;
            SpawnEnemyHeavy();
            SpawnEnemyMedium();
            SpawnEnemyLight();
            wave2 = false;
        }
        if(wave3 == true && killedHeavy == true && killedMedium == true && killedLight == true && isGameOver == false)
        {
            winText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            gameSoundtrack.Stop();
            gameSounds.PlayOneShot(gameWonSound);
            isGameOver = true;
        }
    }

    public void SpawnPowerUp()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, 1f, randomZ);

        Instantiate(powerUp, spawnPos, powerUp.gameObject.transform.rotation);
        playerController.hasPowerup = false;
        gameSounds.PlayOneShot(powerupSpawnSound, 1f);
    }

    public void SpawnEnemyHeavy()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, 5f, randomZ);

        Instantiate(enemies[2], spawnPos, enemies[2].gameObject.transform.rotation);
        killedHeavy = false;
    }

    public void SpawnEnemyMedium()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, 5f, randomZ);

        Instantiate(enemies[1], spawnPos, enemies[1].gameObject.transform.rotation);
        killedMedium = false;
    }

    public void SpawnEnemyLight()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, 5f, randomZ);

        Instantiate(enemies[0], spawnPos, enemies[0].gameObject.transform.rotation);
        killedLight = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Function to call the powerupPickUp audio clip on PowerUpController. Could not create a AudioSource gameSounds variable within this script to then assign
    //it to the AudioSource on SpawnManager and then on PowerUpController script write after Destroy(gameObject) gameSounds.PlayOneShot(powerupPickUp, 2f). Why?
    public void PlayPowerUpPickUpSound()
    {
        gameSounds.PlayOneShot(powerupPickUp, 1f);
    }

    //Function to call the gameoverSound audio clip on PlayerController.
    public void PlayGameOverSound()
    {
        gameSounds.PlayOneShot(gameoverSound, 1f);
        gameSoundtrack.Stop();
    }

    //Function to call the enemyDestroySound audio clip on enemy controllers
    public void PlayEnemyDestroySound()
    {
        gameSounds.PlayOneShot(enemyDestroySound, 2f);
    }

    //Function to call powerupBounceSound audio clip on powerupcontroller
    public void PlayPowerUpBounceSound()
    {
        gameSounds.PlayOneShot(powerupBounceSound, .2f);
    }
}
