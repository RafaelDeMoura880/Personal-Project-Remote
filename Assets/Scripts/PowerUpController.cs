using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    //Speed, Rigidbody and player's gameobject
    public float speed = 1f;
    private Rigidbody powerupRb;
    private GameObject player;
    private SpawnManager spawnManager;
    //FIND A WAY TO COMMUNICATE WITH THE CONSTRAINTOUTOFBOUNDS() ON THE PLAYERCONTROLLER
    public float boundX = 40f;
    public float boundZ = 40f;
    public float enemyForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning to powerup rigidbody component & findind player's gameobject
        powerupRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }
    
    void FixedUpdate()
    {
        //Creates a chasing location based on the players position plus its own positione
        Vector3 lookingPosition = (player.transform.position + transform.position).normalized;
        powerupRb.AddForce(lookingPosition * speed);

        PowerUpMovement();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            spawnManager.PlayPowerUpPickUpSound();
        }
    }

    void PowerUpMovement()
    {
        //Constraints the powerup by adding a Vector3 force to the opposite side of the bounds
        if (transform.position.x > boundX)
        {
            powerupRb.AddForce(Vector3.left * enemyForce, ForceMode.Impulse);
            spawnManager.PlayPowerUpBounceSound();
        }
        if (transform.position.x < -boundX)
        {
            powerupRb.AddForce(Vector3.right * enemyForce, ForceMode.Impulse);
            spawnManager.PlayPowerUpBounceSound();
        }
        if (transform.position.z > boundZ)
        {
            powerupRb.AddForce(Vector3.back * enemyForce, ForceMode.Impulse);
            spawnManager.PlayPowerUpBounceSound();
        }
        if (transform.position.z < -boundZ)
        {
            powerupRb.AddForce(Vector3.forward * enemyForce, ForceMode.Impulse);
            spawnManager.PlayPowerUpBounceSound();
        }
    }
}
