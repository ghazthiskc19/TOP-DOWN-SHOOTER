using UnityEngine;
using System.Collections;

public class EnemyBulletSemi : MonoBehaviour
{
    public AIEnemySemi enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(enemy.getBulletDamage());
            if(collision.gameObject.GetComponent<SanityController>()){
                SanityController sanity = collision.gameObject.GetComponent<SanityController>();
                sanity.lostSanity(8);
                if(sanity.phobiaDarah){
                    healthController.StartDOT(3f);
                }
            }
        }
        if(collision.gameObject.CompareTag("Obstacle")){
             Destroy(gameObject);
        }
       
    }
}
