using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanProjectile : MonoBehaviour
{
    public int damage = 5;
   
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected!");
        //check if pan collides with boss
        if (collision.gameObject.CompareTag("Croissant"))
        {
            BossHealth boss = collision.gameObject.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log("hit");
            }

            Destroy(gameObject); //destroy on collision
        }
    }
}
