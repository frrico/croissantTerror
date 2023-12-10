using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public AudioClip hitSound;

    public Material hitMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material originalMaterial;

    [SerializeField]
    private GameObject crossiants;
    [SerializeField]
    public float swarmerInterval = 3.5f;

    private float cordsx;
    public float cordsy;
    private float cordsz;
    private float bigposition;

    public GameManager gameManager;
    // Start is called before the first frame update

    void Start()
    {
        currentHealth = maxHealth;


        //get skinnedmeshrenderer component
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            originalMaterial = skinnedMeshRenderer.material;
        }

        StartCoroutine(spawncrossiants(swarmerInterval, crossiants));

    }

    private void Update()
    {
        //cordsx = this.transform.position.x;
        ////cordsy = this.transform.position.y;
        //cordsz = this.transform.position.z;

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

    // _____________________Coroutine to coninously spawn crossiants_____________________________//
    private IEnumerator spawncrossiants(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-49f, 42f), cordsy, Random.Range(-21f, 38f)), Quaternion.identity);
        StartCoroutine(spawncrossiants(interval, enemy));
    }
    // _________________________________________________________________________________________//


}

