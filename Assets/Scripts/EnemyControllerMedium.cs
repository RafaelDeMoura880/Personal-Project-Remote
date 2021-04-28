using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerMedium : MonoBehaviour
{
    //Speed, Rigidbody and player's gameobject
    public float speed = 1f;
    private Rigidbody enemyRb;
    private GameObject player;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    public ParticleSystem explosionEffect;
    //FIND A WAY TO COMMUNICATE WITH THE CONSTRAINTOUTOFBOUNDS() ON THE PLAYERCONTROLLER
    public float boundX = 40f;
    public float boundZ = 40f;
    public float enemyForce = 15f;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning to Enemy_Light rigidbody component & findind player's gameobject
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Creates a chasing location based on the players position minus its own position if !hasPowerup
        if (playerController.hasPowerup == false)
        {
            Vector3 lookingPosition = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookingPosition * speed);
            ConstraintWithForce();
        }
        //Creates a running from player location based on the players position plus its own position if hasPowerup
        if (playerController.hasPowerup == true)
        {
            Vector3 lookingPosition = (player.transform.position + transform.position).normalized;
            enemyRb.AddForce(lookingPosition * speed);
        }

        //Constraints the enemy - FIND A WAY TO COMMUNICATE WITH THE CONSTRAINTOUTOFBOUNDS() ON THE PLAYERCONTROLLER
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerController.hasPowerup == true)
        {
            Destroy(gameObject);
            Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
            spawnManager.killedMedium = true;
            spawnManager.PlayEnemyDestroySound();
        }
    }

    void ConstraintWithForce()
    {
        if (transform.position.x > boundX)
        {
            enemyRb.AddForce(Vector3.left * enemyForce, ForceMode.Impulse);
        }
        if (transform.position.x < -boundX)
        {
            enemyRb.AddForce(Vector3.right * enemyForce, ForceMode.Impulse);
        }
        if (transform.position.z > boundZ)
        {
            enemyRb.AddForce(Vector3.back * enemyForce, ForceMode.Impulse);
        }
        if (transform.position.z < -boundZ)
        {
            enemyRb.AddForce(Vector3.forward * enemyForce, ForceMode.Impulse);
        }
    }
}
