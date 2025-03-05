using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera _camera;
    private PlayerShoot _player;

    void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.Find("Player").GetComponent<PlayerShoot>();
    }

    void Update()
    {
        DestroyWhenOffScreen();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyMovement>()){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_player._bulletDamage);
            Destroy(collision.gameObject, .4f);
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
