using UnityEngine;

public class CallEnemyPistol : MonoBehaviour
{
    [SerializeField] private AIEnemyPistol self;
    void OnTriggerStay2D(Collider2D collision){
        if (!self.trackPlayer) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        if(collision.gameObject.GetComponent<AIEnemyPistol>()){
            AIEnemyPistol enemy = collision.gameObject.GetComponent<AIEnemyPistol>();
            if(!enemy.trackPlayer){
            enemy.foundPlayer();  
            } 
        }
            
    }
}
