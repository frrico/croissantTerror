using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    public GameObject transitionImage;

    public void Start()
    {
        transitionImage.SetActive(false);

    }

    public void NextLevel()
    {
        StartCoroutine(PlayGame());
    }
    public IEnumerator PlayGame()
    {
        transitionImage.SetActive(true);
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        transitionAnim.SetTrigger("Start");
    }
    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
