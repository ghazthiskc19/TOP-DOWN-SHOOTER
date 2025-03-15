using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera _camera;
    private PlayerShoot _player;
    private Gun _gun;

    void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.Find("Player").GetComponent<PlayerShoot>();
    }

    void Update()
    {
        DestroyWhenOffScreen();
        _gun = _player.GetComponentInChildren<Gun>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<AIEnemy>()){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_gun._damage);
            var enemy = collision.gameObject.GetComponent<AIEnemy>();
            enemy.foundPlayer();
            Destroy(gameObject, 0.1f);
        }
        else if(collision.gameObject.CompareTag("Obstacle")){
            Destroy(gameObject, 0.1f);
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
