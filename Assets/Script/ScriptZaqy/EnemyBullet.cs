using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Camera _camera;
    public AIEnemy enemy;

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
        if(collision.GetComponent<PlayerMovement>()){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(enemy.bulletDamage);
            // Destroy(collision.gameObject, .4f);
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
