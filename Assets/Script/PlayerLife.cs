using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{


    [SerializeField] private AudioClip dieSound;



    private Rigidbody2D rigibod;
    private Animator anim;
    private AudioSource audioSource;
    
    void Start()
    {
        rigibod = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.CompareTag("Trap")){
            
            Die();
        }
    }

    public void Die(){

        rigibod.bodyType = RigidbodyType2D.Static;
        audioSource.PlayOneShot(dieSound, 0.3f);
        anim.SetTrigger("Death");

        
    }
}
