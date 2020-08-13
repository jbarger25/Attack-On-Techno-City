using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float bulletSpeed = -10.0f;
    public Rigidbody2D rbBulletEnemy;
    public Collider2D bulletCollider;
    public Collider2D enemyCollider;
    public GameObject enemy;
    void Start()
    {
        rbBulletEnemy = this.GetComponent<Rigidbody2D>();
        bulletCollider = this.GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //give projectile velocity and destroy after 1 second
        rbBulletEnemy.velocity = new Vector2(bulletSpeed, 0);
        Destroy(gameObject, 1f);
    }
    //detect collision with player to destroy projectile object
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
