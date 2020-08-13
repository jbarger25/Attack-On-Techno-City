using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 25.0f;
    public Rigidbody2D rbBullet;
    public Collider2D bulletCollider;
    public Animator animator;
    private bool doExplode = false;
    public AudioClip soundEffect;
    public AudioSource audioSource;

    void Start()
    {
        //set object references
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;
        rbBullet = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rbBullet.velocity = new Vector2(bulletSpeed, rbBullet.velocity.y);
    }
    void Awake()
    {
        //give projectile velocity
        rbBullet.velocity = new Vector2(bulletSpeed, rbBullet.velocity.y);
    }
    void Update()
    {
        //destroy the projectile after it has traveled for a certain travel time
        Destroy(gameObject, 2f);
    }
    //detect if projectile has contacted enemies to stop the projectile and play the appropriate sounds and animation
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "TvEnemy")
        {

            doExplode = true;
            bulletCollider.enabled = !bulletCollider.enabled;
            rbBullet.velocity = Vector2.zero;
            
            rbBullet.Sleep();
            audioSource.Play();
            animator.SetBool("DoExplode", doExplode);
        }
        if(col.gameObject.tag == "Obstacle")
        {
            Destroy(col.gameObject);
            doExplode = true;
            bulletCollider.enabled = !bulletCollider.enabled;
            rbBullet.velocity = Vector2.zero;

            rbBullet.Sleep();
            audioSource.Play();
            animator.SetBool("DoExplode", doExplode);
        }
    }
}
