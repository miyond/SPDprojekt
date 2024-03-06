using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{ 
    [SerializeField] private Transform spawnPosition;
    private void OnTriggerEnter2D(Collider2D other){
        
        if (other.CompareTag("Player")){

            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            other.transform.position = spawnPosition.position;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            

        }
    }
}