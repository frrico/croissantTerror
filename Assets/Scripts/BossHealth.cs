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
    public Transform BossTrans;

    public AudioClip hitSound;

    public Material hitMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material originalMaterial;

    [SerializeField]
    private GameObject crossiants;
    [SerializeField]
    private float swarmerInterval = 3.5f;

    private float cordsx;
    private float cordsy;
    private float cordsz;


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

        //StartCoroutine(spawncrossiants(swarmerInterval, crossiants));
        //StartCoroutine(spawncrossiants(swarmerInterval, crossiants, transform.position));
        StartCoroutine(spawnMinis());

    }

    private void Update()
    {
        cordsx = BossTrans.transform.position.x;
        cordsy = BossTrans.transform.position.y;
        cordsz = BossTrans.transform.position.z;
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
    //private IEnumerator spawncrossiants(float interval, GameObject enemy, Vector3 spawnPosition)
    //{
        //yield return new WaitForSeconds(interval);
        //GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        //StartCoroutine(spawncrossiants(interval, enemy, spawnPosition));
        //Debug.Log("spawn");
   // }
    // _________________________________________________________________________________________//

    // THIS SEEMS TO WORK, ALTHOUGH THEY DO NOT CHASE
    private IEnumerator spawnMinis()
    {

        Vector3 Position = new Vector3(cordsx, cordsy -1, cordsz);
        GameObject mini = Instantiate(crossiants, Position, Quaternion.identity);

        Debug.Log($"Mini spawned at {Position}");

        yield return new WaitForSeconds(2f);
        StartCoroutine(spawnMinis());
    }
}
