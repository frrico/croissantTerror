using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveArea : MonoBehaviour

{
    public GameManager gameManager;
  
  void OnTriggerEnter(Collider other)
  {
    // Check if all items are collected
    if (other.CompareTag("Player"))
    {

    
         gameManager.ShowInteractionPrompt();
    }
  }

  void OnTriggerExit(Collider other)
  {
    if(other.CompareTag("Player"))
    {
        gameManager.HideInteractionPrompt();
    }
  }
}
