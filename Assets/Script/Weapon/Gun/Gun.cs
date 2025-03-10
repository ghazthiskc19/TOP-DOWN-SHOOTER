using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public string _weaponName;
    public int _magazineSize;
    public int _currentAmmo;
    public int _leftOverAmmo;
    public float _fireRate;
    public float _reloadTime;
    public float _damage;
    public float _bulletSpeed;
    public GameObject _bullet;
    private PlayerShoot _playerShoot;
    private PlayerMovement PlayerMovement; 
    private bool isReloading = false;
    public bool _isReloading => isReloading;
    private bool _pressReloadKey;
    private float timer = 0;

    private void Awake()
    {
        // _playerShoot = FindAnyObjectByType<PlayerShoot>();
        _playerShoot = GetComponentInParent<PlayerShoot>();
        PlayerMovement = GetComponentInParent<PlayerMovement>();
    }
    public void OnReload(InputValue input){
        if(input.isPressed){
            _pressReloadKey = true; 
        }
    }
    private void Update()
    {
        if(_pressReloadKey && !isReloading){
            isReloading = true;
            Debug.Log(_pressReloadKey);
            Reload();
        }

        if(_currentAmmo == 0  && !isReloading && PlayerMovement._isAiming){
            isReloading = true;
            Reload();
        }
    }
    public void Fire(){
        _playerShoot = GetComponentInParent<PlayerShoot>();
        if(_currentAmmo > 0)
        {
             _playerShoot.FireBullet(_bullet);
            _currentAmmo--;
        }
    }

    public void Reload()
    {
        if(timer < _reloadTime){
            timer += Time.deltaTime;
        }else{
            if(_leftOverAmmo > _magazineSize){
                _leftOverAmmo -= _magazineSize - (_currentAmmo % _magazineSize);
                _currentAmmo += _magazineSize - (_currentAmmo % _magazineSize);
            }else if(_leftOverAmmo > 0){
                _leftOverAmmo -= _leftOverAmmo - (_currentAmmo % _leftOverAmmo);
                _currentAmmo += _leftOverAmmo - (_currentAmmo % _leftOverAmmo);
            }
            isReloading = false;
            _pressReloadKey = false;
            _playerShoot.UpdateBulletText();
        }
    }


    
}
