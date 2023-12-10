using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public AudioClip hitSound;

    public Material hitMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material originalMaterial;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
       

        //get skinnedmeshrenderer component
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if(skinnedMeshRenderer != null)
        {
            originalMaterial = skinnedMeshRenderer.material;
        }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        //highlight red when hit
        if(hitMaterial != null && skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.material = hitMaterial;
            Invoke("ResetMaterial", 0.5f);
        }

        if(currentHealth <= 0)
        {
            BossDie();
        }
    }

    void ResetMaterial()
    {
        //reset material back to original
        skinnedMeshRenderer.material = originalMaterial;
    }

    void BossDie()
    {
        Destroy(gameObject);
        gameManager.WinGame();
        
    }
}

