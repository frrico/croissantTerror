using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
   public void restartGame()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
   }
    
}
