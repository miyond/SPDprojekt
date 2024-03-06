using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardForce = 100f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float Hitpoints;
    [SerializeField] private float MaxHitpoints = 3;

   // [SerializeField] private AudioClip slimeSound;
    
    private SpriteRenderer rend;
    private Animator anim;
    //private AudioSource audioSource;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Hitpoints = MaxHitpoints;
        anim = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }


    void FixedUpdate()
    {
        transform.Translate(new Vector2 (moveSpeed, 0) * Time.deltaTime);
        
        if(moveSpeed > 0){

            rend.flipX = true;
        }

        if(moveSpeed < 0){

            rend.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other){

        if(other.gameObject.CompareTag("EnemyBlock")){

            moveSpeed = -moveSpeed;
        }

        if(other.gameObject.CompareTag("Enemy")){

            moveSpeed = -moveSpeed;
        }

        if(other.gameObject.CompareTag("Player")){

            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damageGiven);

            if(other.transform.position.x > transform.position.x){

                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockbackForce, upwardForce);

            }else{
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockbackForce, upwardForce);
            }
        }

        }
      public void TakeHit(float damage){

        Hitpoints -= damage;
        if(Hitpoints <= 0){

            Destroy(gameObject);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other){

        if(other.CompareTag("Player")){

            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));
            TakeHit(1);
            
            
        
        
        
}
}
}
