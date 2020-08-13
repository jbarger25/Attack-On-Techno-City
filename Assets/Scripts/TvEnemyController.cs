using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvEnemyController : MonoBehaviour
{
    private float health = 30;
    public Rigidbody2D rb2d;
    private float lastMovement = 0.0f;
    private float interval = 1.5f;
    private int moveDirection = -1;
    public float activationDistance = 10;
    public float chargeSpeed = 15;
    public GameObject player;
    public Animator animator;
    public AudioClip soundEffect1;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect1;
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");//find player object to know at what distance to attack
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        ChargeAttack();
        if(Time.time >= lastMovement)//idle movement back and forth
        {
            EnemyMove(moveDirection);
            moveDirection *= -1;
            lastMovement += interval;
        }
    }

    private void EnemyMove(int direction)
    {
        rb2d.velocity = new Vector2(5f * direction, rb2d.velocity.y);
    }
    //find distance from player
    private float DistanceDifference()
    {
        float difference = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(player.transform.position.x));
        return difference;
    }
    //move to player to do damage
    private void ChargeAttack()
    {
        if(DistanceDifference() <= activationDistance)
        {
            rb2d.AddForce( new Vector2(chargeSpeed * -1, rb2d.velocity.y));
        }
    }
    private void CheckHealth()
    {
        if(health <= 0)
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        audioSource.Play();
        animator.SetBool("DoExplode", true);
    }
    //detect player projectile to decrease health
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile")
        {
            rb2d.velocity = new Vector2(1, 4);
            health -= 10;
        }
    }
}
