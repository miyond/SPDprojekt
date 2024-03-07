using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AudioClip jumpSound, pickupSound, slidingSound, takeHitSound, dieSound, slimeSound;
    [SerializeField] private GameObject slidingParticles;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColor;
    [SerializeField] private Color orangeHealth, redHealth;
    [SerializeField] private TMP_Text bunnyText;


    private float horizontalValue;
    private float rayDistance = 0.25f;
    private bool isOnGround;
    private bool canMove;
//Wall sliding
    private bool isWallSliding;
    private float wallSlidingSpeed = 2.5f;
//Wall jumping

    /* private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f); */

//Health and collectibles
    private int startingHealth = 3;
    private int currentHealth = 0;
    public int bunniesCollected = 0;

    private Rigidbody2D rigibod;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;
    
    

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        currentHealth = startingHealth;
        bunnyText.text = "" + bunniesCollected;
        rigibod = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){

        //Move left and rightCheckIfOnGroundIs
        
        horizontalValue = Input.GetAxis("Horizontal");

        if(horizontalValue < 0){

            FlipSprite(true);

        }
        if(horizontalValue > 0){

            FlipSprite(false);
        }



        if(Input.GetButtonDown("Jump") && CheckIfOnGround() == true) {

            Jump();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rigibod.velocity.x));
        anim.SetFloat("VerticalSpeed", rigibod.velocity.y);
        anim.SetBool("IsGrounded", CheckIfOnGround());


        WallSlide();

        //WallJump();


    }

    private void FixedUpdate(){

        if(!canMove){

            return;
        }
        
            rigibod.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rigibod.velocity.y);

    }
    private void OnTriggerEnter2D(Collider2D other){

        if (other.CompareTag("Bunny")){

            Destroy(other.gameObject);
            bunniesCollected++;
            bunnyText.text = "" + bunniesCollected;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(pickupSound, 0.6f);
            
        }

        if(other.CompareTag("Health")){

            RestoreHealth(other.gameObject);
            
        }

        if(other.CompareTag("Enemy")){
            audioSource.PlayOneShot(slimeSound, 0.5f);
        }
    }
    private void FlipSprite(bool direction){

        rend.flipX = direction;


    }
    private void Jump(){

        rigibod.AddForce(new Vector2(0, jumpForce));
        audioSource.PlayOneShot(jumpSound, 0.3f);

    }


    private bool IsWalled(){

        return Physics2D.OverlapCircle(wallCheck.position, 0.6f, wallLayer);
        

    }

    private void WallSlide(){

        if(IsWalled() && !CheckIfOnGround() && horizontalValue != 0f){

            
            isWallSliding = true;
            rigibod.velocity = new Vector2(rigibod.velocity.x, Mathf.Clamp(rigibod.velocity.y, -wallSlidingSpeed, float.MaxValue));
            Instantiate(slidingParticles, transform.position, Quaternion.identity);
            anim.SetBool("Sliding", true);

            if(!audioSource.isPlaying)
            audioSource.Play();

        }else{

            isWallSliding = false;
            audioSource.Stop();
            anim.SetBool("Sliding", false);
            
            
        }
    }
    public void TakeDamage(int damageAmount){

        currentHealth -= damageAmount;
        audioSource.PlayOneShot(takeHitSound, 0.5f);
        UpdateHealthBar();

        if(currentHealth <= 0){
            
            audioSource.PlayOneShot(dieSound, 0.3f);
            anim.SetTrigger("Death");
            Invoke("Respawn", 5f);
        }

    }
    public void TakeKnockback(float knockbackForce, float upwards){

        canMove = false;
        rigibod.AddForce(new Vector2(knockbackForce, upwards));
        Invoke("CanMoveAgain", 0.25f);

    }
    private void CanMoveAgain(){

        canMove = true;
    }
    public void Respawn(){


        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentHealth = startingHealth;
        UpdateHealthBar();
        transform.position = spawnPosition.position;
        rigibod.velocity = Vector2.zero;

    }
    private void RestoreHealth(GameObject healthPickup){

        if(currentHealth >= startingHealth){
            return;
        }else{

            int healthToRestore = healthPickup.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            audioSource.PlayOneShot(pickupSound, 0.6f);
            Destroy(healthPickup);

            if(currentHealth >= startingHealth){

                currentHealth = startingHealth;
            }
        }

}
    private void UpdateHealthBar(){

        healthSlider.value = currentHealth;

        if(currentHealth >= 2){

            fillColor.color = orangeHealth;

        }else{
            fillColor.color = redHealth;

        }
    }
    private bool CheckIfOnGround(){
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if(leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground")){
            return true;
        }
        else
        {
            return false;
        }
    }
}


