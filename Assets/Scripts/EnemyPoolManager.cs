using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public GameObject miniCroissant;
    public int poolSize = 10;

    private List<GameObject> miniEnemies;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        miniEnemies = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject miniEnemy = Instantiate(miniCroissant);
            miniEnemy.SetActive(false);
            miniEnemies.Add(miniEnemy);
        }
    }

    public GameObject GetPooledEnemy()
    {
        foreach (GameObject miniEnemy in miniEnemies)
        {
            if (!miniEnemy.activeInHierarchy)
            {
                return miniEnemy;
            }
        }

        return null;
    }
}
