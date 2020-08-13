using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private bool doPlayerIdleRight = true;
    private bool doPlayerIdleLeft = false;
    private bool doPlayerShootRight = false;
    private bool doPlayerShootLeft = false;
    private bool doPlayerRunRight = false;
    private bool doPlayerRunLeft = false;
    private bool facingRight = true;
    private Rigidbody2D rb2d;
    public float moveSpeed = 50;
    public float jumpHeight = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public GameController GameController;
    public GameObject PlayerProjectileRight;
    public GameObject PlayerProjectileLeft;
    public GameUI gameUI;
    public float playerHealth = 1f;
    public float playerEnergy = 1f;
    private Vector2 ProjectilePos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;
    public int levelProgress = 0;
    public static PlayerController instance = null;
    public GameObject Bounds;
    public Collider2D HiddenCollider;
    public Collider2D playerCollider;
    private bool isGrounded;
    public AudioClip soundEffect1;
    public AudioClip soundEffect2;
    public AudioClip soundEffect3;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect1;
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();
        animator.SetBool("DoPlayerIdleRight", doPlayerIdleRight);
        rb2d = GetComponent<Rigidbody2D>();
        Bounds = GameObject.Find("BoundsColliders");
        HiddenCollider = Bounds.GetComponent<Collider2D>();//get reference to collider used to contain enemies so player collider can ignore
        playerCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, HiddenCollider);
        gameUI = GameObject.Find("HealthBar").GetComponent<GameUI>();
        gameUI.SetHealthSize(playerHealth);
        gameUI.SetEnergySize(playerEnergy);

    }
    //Singleton implementation
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        //check for idle status to play animation in appropriate direction
        if (!Input.anyKey)
        {
            if (facingRight)
            {
                doPlayerIdleRight = true;
                animator.SetBool("DoPlayerIdleRight", doPlayerIdleRight);
            }
            else
            {
                doPlayerShootLeft = false;
                animator.SetBool("DoPlayerShootLeft", doPlayerShootLeft);
            }
            doPlayerShootRight = false;
            animator.SetBool("DoPlayerShootRight", doPlayerShootRight);
            playerRun(false, 1);
            playerRun(false, -1);
        }
        //check for fire input and play animation in appropriate direction
        else if (Input.GetKey(KeyCode.Space))
        {
            audioSource.clip = soundEffect3;
            audioSource.Play();
            if (facingRight)
            {
                doPlayerShootRight = true;
                animator.SetBool("DoPlayerShootRight", doPlayerShootRight);
            }
            else
            {
                doPlayerShootLeft = true;
                animator.SetBool("DoPlayerShootLeft", doPlayerShootLeft);
            }
            //pacing for firerate
            if(Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Invoke("playerShoot", 0.15f);
            }
        }
        //checking for input movement to play appropriate animation and call proper method
        else if (Input.GetKey(KeyCode.D))
        {
            facingRight = true;
            doPlayerIdleLeft = false;
            animator.SetBool("DoPlayerIdleLeft", doPlayerIdleLeft);
            playerRun(true, 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            facingRight = false;
            doPlayerIdleLeft = true;
            animator.SetBool("DoPlayerIdleLeft", doPlayerIdleLeft);
            playerRun(true, -1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)//ensure player cannot "fly" through the levels
            {
                playerJump();
            }
        }
        //ensure player has health and has not fallen off the map
        checkPlayerStatus();
        //ensure object references are not lost
        if(Bounds == null)
        {
            Bounds = GameObject.Find("BoundsColliders");
            HiddenCollider = Bounds.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(playerCollider, HiddenCollider);
        }
        if (gameUI == null)
        {
            gameUI = GameObject.Find("HealthBar").GetComponent<GameUI>();
            gameUI.SetHealthSize(playerHealth);
            gameUI.SetEnergySize(playerEnergy);
        }
        if(animator == null)
        {
            animator = GetComponent<Animator>();
            animator.SetBool("DoPlayerIdleRight", doPlayerIdleRight);
        }
        //input for healing
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (playerEnergy > 0.0f)
            {
                audioSource.clip = soundEffect2;
                audioSource.Play();
                playerEnergy -= 0.5f;
                gameUI.SetEnergySize(playerEnergy);
                playerHealth += 0.3f;
                if(playerHealth > 1.0f)
                {
                    playerHealth = 1.0f;
                }
                gameUI.SetHealthSize(playerHealth);
            }
        }
    }

    public void playerRun(bool doRun, int direction)
    {
        if (direction == 1)
        {
            doPlayerRunRight = doRun;
            animator.SetBool("DoPlayerRunRight", doPlayerRunRight);
        }
        else if (direction == -1)
        {
            doPlayerRunLeft = doRun;
            animator.SetBool("DoPlayerRunLeft", doPlayerRunLeft);
        }
        if (doRun)
        {
            rb2d.velocity = new Vector2(moveSpeed*direction, rb2d.velocity.y);
        }
    }

    public void playerJump()
    {
        rb2d.velocity = Vector2.up * jumpHeight;
        if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb2d.velocity.y >0 && !Input.GetKey(KeyCode.W))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void playerShoot()
    {
        
        ProjectilePos = transform.position;
        if (facingRight == true)
        {
            ProjectilePos += new Vector2(+0.9f, +0.2f);
            Instantiate(PlayerProjectileRight, ProjectilePos, Quaternion.identity);
        }
        else
        {
            ProjectilePos += new Vector2(-0.5f, +0.2f);
            Instantiate(PlayerProjectileLeft, ProjectilePos, Quaternion.identity);
        }
    }

    void playerDie()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
    //ensure player has health and has not fallen off the map
    void checkPlayerStatus()
    {
        if (playerHealth == 0)
        {
            playerDie();
        }
        if(transform.position.y <= -20)
        {
            transform.position = new Vector3(-9f, -2.8f, 1f);
            playerHealth -= 0.2f;
            gameUI.SetHealthSize(playerHealth);
        }
    }
    //detect collisions with enemies and projectiles and decrease heath accordingly
    //also detect collisions with game triggers like end of levels or energy pickups
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyProjectile")
        {
            rb2d.velocity = new Vector2(-2, 10);
            playerHealth -= 0.1f;
            gameUI.SetHealthSize(playerHealth);
        }

        if(collision.gameObject.tag == "LevelEnd")
        {
            levelProgress ++;
            if (levelProgress == 5)
            {
                transform.position = new Vector3(0f, 0f, 1f);
                GameController.ProgressLevel(levelProgress);
            }
            else
            {
                transform.position = new Vector3(-9f, -2.8f, 1f);
                GameController.ProgressLevel(levelProgress);
            }
        }
        if (collision.gameObject.tag == "TvEnemy")
        {
            rb2d.velocity = new Vector2(-2, 10);
            playerHealth -= 0.2f;
            gameUI.SetHealthSize(playerHealth);
        }
        if(collision.gameObject.tag == "EnergyPickup")
        {
            audioSource.clip = soundEffect1;
            audioSource.Play();
            playerEnergy += 0.5f;
            if(playerEnergy > 1f)
            {
                playerEnergy = 1f;
            }
            gameUI.SetEnergySize(playerEnergy);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    //detect when player is in mid air to prevent "flying"
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }



}
