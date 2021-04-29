using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerMedium : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody enemyRb;
    private GameObject player;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    public ParticleSystem explosionEffect;
    public float boundX;
    public float boundZ;
    public float enemyForce;

    void Start()
    {
        //Assigning to Enemy_Light rigidbody component & findind player's gameobject
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    void FixedUpdate()
    {
        if (playerController.hasPowerup == false) //Creates a chasing location based on the players position minus its own position if !hasPowerup
        {
            Vector3 lookingPosition = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookingPosition * speed);
            ConstraintWithForce();
        }
        else //Creates a running from player location based on the players position plus its own position if hasPowerup
        {
            Vector3 lookingPosition = (player.transform.position + transform.position).normalized;
            enemyRb.AddForce(lookingPosition * speed);
            ConstraintNoForce();
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

    void ConstraintNoForce()
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
}