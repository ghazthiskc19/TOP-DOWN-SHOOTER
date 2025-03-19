using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private float _amountDamage;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>()){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_amountDamage);
        }
    }

}
