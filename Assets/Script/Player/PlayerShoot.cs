using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _timeBetweenAttack;
    public float _bulletDamage;
    private CrosshairController _crosshairController;
    private Transform _gunOffset;
    private float timer;
    private bool _continousAttack;
    private bool _fireSingle;
    void Awake()
    {
        _gunOffset = transform.GetChild(1);
        _crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(_continousAttack || _fireSingle){
                timer -= Time.deltaTime;
            if( timer < 0){
                _crosshairController.ExpandCrosshair();
                FireBullet();
                _crosshairController.ResetCrosshair();
                timer = _timeBetweenAttack;
                _fireSingle = false;
            }
        }else{
            timer = 0;
        }
    }
    void OnAttack(InputValue input){
        _continousAttack = input.isPressed;
        if(input.isPressed){
            _fireSingle = true;
        }
    }
    void FireBullet(){
        GameObject bullet = Instantiate(_bulletPrefabs, _gunOffset.position, transform.rotation);
        Rigidbody2D _rbBullet = bullet.GetComponent<Rigidbody2D>();
        _rbBullet.linearVelocity = _bulletSpeed * transform.up;
    }
}
