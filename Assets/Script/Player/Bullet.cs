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
        if(collision.gameObject.CompareTag("Enemy")){
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_gun._damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Obstacle")){
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
