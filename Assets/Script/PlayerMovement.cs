using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AudioClip jumpSound, pickupSound;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColor;
    [SerializeField] private Color orangeHealth, redHealth;
    [SerializeField] private TMP_Text bunnyText;


    private float horizontalValue;
    private float rayDistance = 0.25f;
    private bool isOnGround;
    private bool canMove;
    private int startingHealth = 3;
    private int currentHealth = 0;
    private int bunniesCollected = 0;

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

        //Move left and right
        
        horizontalValue = Input.GetAxis("Horizontal");

        if(horizontalValue < 0){

            FlipSprite(true);

        }
        if(horizontalValue > 0){

            FlipSprite(false);
        }

        

        //Jump


        if(Input.GetButtonDown("Jump") && CheckIfOnGround() == true) {

            Jump();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rigibod.velocity.x ));
        anim.SetFloat("VerticalSpeed", rigibod.velocity.y);
        anim.SetBool("IsGrounded", CheckIfOnGround());


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
            audioSource.PlayOneShot(pickupSound, 0.4f);
            
        }

        if(other.CompareTag("Health")){

            RestoreHealth(other.gameObject);
        }
    }

    private void FlipSprite(bool direction){

        rend.flipX = direction;


    }
    private void Jump(){

        rigibod.AddForce(new Vector2(0, jumpForce));
        audioSource.PlayOneShot(jumpSound, 0.3f);

    }
    public void TakeDamage(int damageAmount){

        currentHealth -= damageAmount;
        UpdateHealthBar();

        if(currentHealth <= 0){

            Respawn();
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

    private void Respawn(){

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

