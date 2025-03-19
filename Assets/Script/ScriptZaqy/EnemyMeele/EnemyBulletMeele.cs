using UnityEngine;

public class EnemyBulletMeele : MonoBehaviour
{
    private Camera _camera;
    public AIEnemyMeele enemy;

    void Awake()
    {
        _camera = Camera.main;
        // _player = GameObject.Find("Player").GetComponent<PlayerShoot>();
    }

    void Update()
    {
        DestroyWhenOffScreen();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(enemy.getBulletDamage());
            if(GetComponent<SanityController>()){
                SanityController sanity = gameObject.GetComponent<SanityController>();
                sanity.lostSanity(8);
            }
        }
        if(collision.gameObject.CompareTag("Obstacle")){
             Destroy(gameObject);
        }
       
    }

    void DestroyWhenOffScreen(){
        Vector3 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if(
            screenPosition.x < 0 || screenPosition.y < 0 || 
            screenPosition.x > _camera.pixelWidth || screenPosition.y > _camera.pixelHeight
        )
        {
            Destroy(gameObject);
        }
    }
}
