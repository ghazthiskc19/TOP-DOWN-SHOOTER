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
    private bool isReloading = false;
    public bool _isReloading => isReloading;
    private bool _pressReloadKey;
    private void Awake()
    {
        // _playerShoot = FindAnyObjectByType<PlayerShoot>();
        _playerShoot = GetComponentInParent<PlayerShoot>();
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
            StartCoroutine(Reload());
        }
    }
    public virtual void Fire(){
        _playerShoot = GetComponentInParent<PlayerShoot>();
        if(_currentAmmo > 0)
        {
            // if(_playerShoot != null)
             _playerShoot.FireBullet(_bullet);
            _currentAmmo--;
        }

        if(_currentAmmo == 0  && !isReloading){
            isReloading = true;
            StartCoroutine(Reload());
        }
    }

    public virtual IEnumerator Reload()
    {
        Debug.Log("Masuk sini");
        yield return new WaitForSeconds(_reloadTime);
        if(_leftOverAmmo > _magazineSize){
            _currentAmmo += _magazineSize - (_currentAmmo % _magazineSize);
            _leftOverAmmo -= _magazineSize - (_currentAmmo % _magazineSize);
        }else if(_leftOverAmmo > 0){
            _currentAmmo += _leftOverAmmo - (_currentAmmo % _leftOverAmmo);
            _leftOverAmmo -= _leftOverAmmo - (_currentAmmo % _leftOverAmmo);
        }
        isReloading = false;
        _pressReloadKey = false;
        _playerShoot.UpdateBulletText();
    }


    
}
