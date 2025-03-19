using UnityEngine;

public class CallEnemySemi : MonoBehaviour
{
    [SerializeField] private AIEnemySemi self;
    void OnTriggerStay2D(Collider2D collision){
        if (!self.trackPlayer) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        if(self.trackPlayer && collision.gameObject.CompareTag("Enemy")){
            if(collision.gameObject.GetComponent<AIEnemyPistol>()){
                AIEnemyPistol enemy = collision.gameObject.GetComponent<AIEnemyPistol>();
                if(!enemy.trackPlayer){
                    enemy.foundPlayer();  
                }
            }
            if(collision.gameObject.GetComponent<AIEnemySMG>()){
                AIEnemySMG enemy = collision.gameObject.GetComponent<AIEnemySMG>();
                if(!enemy.trackPlayer){
                    enemy.foundPlayer();  
                }
            }
            if(collision.gameObject.GetComponent<AIEnemySemi>()){
                AIEnemySemi enemy = collision.gameObject.GetComponent<AIEnemySemi>();
                if(!enemy.trackPlayer){
                    enemy.foundPlayer();  
                }
            }
            if(collision.gameObject.GetComponent<AIEnemyMeele>()){
                AIEnemyMeele enemy = collision.gameObject.GetComponent<AIEnemyMeele>();
                if(!enemy.trackPlayer){
                    enemy.foundPlayer();  
                }
            }
            if(collision.gameObject.GetComponent<AIEnemySniper>()){
                AIEnemySniper enemy = collision.gameObject.GetComponent<AIEnemySniper>();
                if(!enemy.trackPlayer){
                    enemy.foundPlayer();  
                }
            }
            
        }
    }
}
