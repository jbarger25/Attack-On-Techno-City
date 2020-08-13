using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : MonoBehaviour
{
    public int health = 20;
    public float movementRangeMax = 5f;
    public float movementRangeMin = 5f;
    public float moveSpeed = 5; 
    public Collider2D col;
    public Rigidbody2D rb2d;
    private Vector2 moveVector;
    private Vector2 projectilePos;
    public GameObject player;
    public GameObject EnemyProjectileLeft;
    private float lastShot;
    public float shotInterval = 1.5f;
    public Animator animator;
    public AudioClip soundEffect1;
    public AudioClip soundEffect2;
    public AudioSource audioSource;
    //instantiate class varables and get references to game objects
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect1;
        col = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        lastShot = Time.time;

    }


    void Update()
    {
        enemyMove();
            //check time and compare to previous shooting time to fire at a certain pace
        if (Time.time > lastShot)
        {
            enemyShoot();
            lastShot += shotInterval;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void enemyMove()
    {
        //float dist = distanceDifference();
        if (distanceDifference() >= movementRangeMax)
        {
            rb2d.velocity = new Vector2(moveSpeed * -1, rb2d.velocity.y);
        }
        else if (distanceDifference() <= movementRangeMin)
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }

    }
    private void Die()
    {
        audioSource.clip = soundEffect1;
        audioSource.Play();
        animator.SetBool("DoExplode", true);
    }
    //calculate distance from player for movement method
    private float distanceDifference()
    {
        float difference = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(player.transform.position.x));
        return difference;
    }
    //instantiate projectile and play audio
    void enemyShoot()
    {
        audioSource.clip = soundEffect2;
        audioSource.Play();
        projectilePos = transform.position;
        {
            projectilePos += new Vector2(-3f, +0.2f);
            Instantiate(EnemyProjectileLeft, projectilePos, Quaternion.identity);
        }
    }
    //detect when hit by player projectile, add movement for feel and decrease health
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerProjectile")
        {
            rb2d.velocity = new Vector2(2, 10);
            health -= 10;
        }
    }
}
