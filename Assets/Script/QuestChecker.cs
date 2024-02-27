using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{

    [SerializeField] private GameObject dialogueBox, finishedText, unfinishedText;
    [SerializeField] private int questGoal = 10;
    [SerializeField] private int levelToLoad;

    private bool levelIsLoading = false;

    private void OnTriggerEnter2D(Collider2D other){

        if(other.CompareTag("Player")){

            if(other.GetComponent<PlayerMovement>().bunniesCollected >= questGoal){

                dialogueBox.SetActive(true);
                finishedText.SetActive(true);
                Invoke("LoadNextLevel", 2.0f);
                levelIsLoading = true;
                //TODO: Change level

            }else{

                dialogueBox.SetActive(true);
                unfinishedText.SetActive(true);
            }
        }
    }

    private void LoadNextLevel(){

        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerExit2D(Collider2D other){

        if(other.CompareTag("Player") && !levelIsLoading){

            dialogueBox.SetActive(false);
            finishedText.SetActive(false);
            unfinishedText.SetActive(false);

        }
    }
}