using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class    PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private float _attackDelay;
    public float _bulletDamage;
    private CrosshairController _crosshairController;
    private Transform _gunOffset;
    private float timer;
    private bool _continousAttack;
    private bool _fireSingle;
    private Camera _camera;
    private PlayerMovement _playerMovement;
    private WeaponHolder _weaponHolder;
    public TMP_Text _currentBullet;
    public TMP_Text _leftOverBullet;
    void Awake()
    {
        _gunOffset = transform.GetChild(1);
        _crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
        _camera = Camera.main;
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
    }
    void Update()
    {
        UpdateBulletText();
        if(_playerMovement._isAiming){
            if(_weaponHolder.GetCurrentWeapon()._isReloading){
                _crosshairController.SetReloadEffect();
            }
            if(_continousAttack || _fireSingle){
                    timer -= Time.deltaTime;
                if( timer < 0){
                    _crosshairController.ExpandCrosshair();
                    _weaponHolder.GetCurrentWeapon().Fire();
                    _crosshairController.ResetCrosshair();
                    timer = _weaponHolder.GetCurrentWeapon()._fireRate;
                    _fireSingle = false;
                }
            }else{
                timer = 0;
            }
        }
    }
    void OnAttack(InputValue input){
        if(!_playerMovement._isAiming) return;
        _continousAttack = input.isPressed;
        if(input.isPressed){
            _fireSingle = true;
        }
    }
    void OnAim(InputValue input){
        _playerMovement._isAiming = !_playerMovement._isAiming;
        _playerMovement.SetAiming(_playerMovement._isAiming);
    }

    public void FireBullet(GameObject _bulletPrefabs){
        if(_weaponHolder.GetCurrentWeapon()._currentAmmo <= 0) return;
        
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(_bulletPrefabs, _gunOffset.position, Quaternion.Euler(0, 0, angle));
        bullet.transform.up = direction;
        Rigidbody2D _rbBullet = bullet.GetComponent<Rigidbody2D>();
        _rbBullet.linearVelocity = _weaponHolder.GetCurrentWeapon()._bulletSpeed * direction;
    }
    public void UpdateBulletText()
    {
        _currentBullet.text = $"{_weaponHolder.GetCurrentWeapon()._currentAmmo} /";
        _leftOverBullet.text = $"{_weaponHolder.GetCurrentWeapon()._leftOverAmmo}";
    }

}
