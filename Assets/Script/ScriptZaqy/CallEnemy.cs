using UnityEngine;

public class CallEnemy : MonoBehaviour
{
    [SerializeField] private AIEnemy self;
    void OnTriggerStay2D(Collider2D collision){
        if(self.trackPlayer && collision.gameObject.CompareTag("Enemy")){
            AIEnemy enemy = collision.gameObject.GetComponent<AIEnemy>();
            if(!enemy.trackPlayer){
              enemy.foundPlayer();  
            }
        }
    }
}
