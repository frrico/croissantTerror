//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySpanwer : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject crossiants;
//    [SerializeField]
//    private float swarmerInterval = 3.5f;

//    private GameObject Enemy;
//    public float cordsy;


//    void Start()
//    {

//        StartCoroutine(spawncrossiants(swarmerInterval, crossiants));

//    }


//    // Update is called once per frame
//    void Update()
//    {
//    }

//    // _____________________Coroutine to coninously spawn crossiants_____________________________//
//    private IEnumerator spawncrossiants(float interval, GameObject enemy)
//    {
//        yield return new WaitForSeconds(interval);
//        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-49f, 42f), cordsy, Random.Range(-21f,38f)), Quaternion.identity);
//        StartCoroutine(spawncrossiants(interval, enemy));
//    }
//    // _________________________________________________________________________________________//

//}
