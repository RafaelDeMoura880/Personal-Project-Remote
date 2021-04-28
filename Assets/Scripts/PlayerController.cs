using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float boundX = 40f;
    public float boundZ = 40f;
    private bool onBound;
    public bool hasPowerup;
    private bool isAlive = true;
    private BoxCollider ground;
    private Rigidbody playerRb;
    private GameObject powerUp;
    private SpawnManager spawnManager;
    public GameObject gameoverText;
    public GameObject restartButton;
    public TextMeshProUGUI timerText;
    private float timer = 6f;
    
    void Start()
    {
        //Assigns the Rigidbody component to the playerRb variable
        playerRb = GetComponent<Rigidbody>();

        //Finds the SpawnManager script
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        //Finds the powerup
        powerUp = GameObject.Find("Powerup");
    }

    void FixedUpdate()
    {
        PlayerMovement();

        ConstraintOutOfBounds();
    }


    //Horizontal & Vertical Inputs
    void PlayerMovement()
    {
        if (isAlive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime, Space.World);
            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime, Space.World);
        }
    }


    //X & Z boundaries - maybe find a cleaner way?
    public void ConstraintOutOfBounds()
    {
        if (transform.position.x > boundX)
        {
            transform.position = new Vector3(boundX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -boundX)
        {
            transform.position = new Vector3(-boundX, transform.position.y, transform.position.z);
        }
        if (transform.position.z > boundZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundZ);
        }
        if (transform.position.z < -boundZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundZ);
        }
        
    }


    //Manages phisics collision with Enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasPowerup)
        {
            isAlive = false;
            gameoverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            spawnManager.PlayGameOverSound();
        }
        if (collision.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            StartCoroutine(PowerUpCountDownRoutine());
            timerText.gameObject.SetActive(true);
        }
    }

    //Timer for the powerup duration. Update() keeps checking when to start and when to reset
    public void TimerCountdown()
    {
        timer -= Time.deltaTime;
        timerText.text = "" + (int)timer;
    }

    private void Update()
    {
        if(hasPowerup == true)
        {
            TimerCountdown();
        }
        if (hasPowerup == false)
        {
            timer = 6f;
        }
    }

    //Coroutine to countdown powerup time and set it to false after end of timer.
    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(5);
        spawnManager.SpawnPowerUp();
        timerText.gameObject.SetActive(false);
    }
    
}
