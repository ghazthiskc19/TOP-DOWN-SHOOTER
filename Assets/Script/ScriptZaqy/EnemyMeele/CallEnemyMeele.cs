using UnityEngine;

public class CallEnemyMeele : MonoBehaviour
{
    [SerializeField] private AIEnemyMeele self;
    void OnTriggerStay2D(Collider2D collision){
        if(self.trackPlayer && collision.gameObject.CompareTag("Enemy")){
            AIEnemyPistol enemy = collision.gameObject.GetComponent<AIEnemyPistol>();
            if(!enemy.trackPlayer){
              enemy.foundPlayer();  
            }
        }
    }
}
