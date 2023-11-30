using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public int totalItems;
    public int collectedItems;

    public Transform objectiveArea;

    public TextMeshProUGUI interactionPrompt;

    public string winScene = "WinScene";

    void Start()
    {
        collectedItems = 0;
        //set interaction text to hidden
        if(interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    public void ItemCollected()
    {
        collectedItems++;

    }

    public void WinGame()
    {
        SceneManager.LoadScene(winScene);
        
    }

    public void TryInteractWithObjective()
    {
        if (collectedItems == totalItems)
        {
            // Player has all items, trigger win condition
            
            WinGame();
        }
        else
        {
            // Player does not have all items, show a message or take other actions
            Debug.Log("?");
        }
    }

    public void ShowInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    bool IsPlayerInArea()
{
    if (objectiveArea != null)
    {
        Collider playerCollider = GetComponent<Collider>();
        Collider objectiveCollider = objectiveArea.GetComponent<Collider>();

        if (playerCollider != null && objectiveCollider != null)
        {
            // Use ClosestPointOnBounds to get a point on the player collider's bounds
            Vector3 closestPoint = playerCollider.ClosestPointOnBounds(objectiveCollider.bounds.center);

            // Check if the closest point is within the objective collider
            return objectiveCollider.bounds.Contains(closestPoint);
        }
        else
        {
            return false;
        }
    }
    return false;
}


     

}




