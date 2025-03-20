using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class    PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private Sprite[] _spriteAimPistol;
    [SerializeField] private LayerMask _enemyLayer;
    public Vector2 MeeleSize = new Vector2(1.5f, 1.0f);
    public float MeeleRange = 1.0f;
    public float MeeleDuration = 1f;
    public float MeeleDamage = 30f;
    private SpriteRenderer _spriteRenderer;
    private CrosshairController _crosshairController;
    private Animator _animator;
    private Animator _meeleAnimator;
    private Transform _gunOffset;
    private Gun _gun;
    private float timer;
    private bool _continousAttack;
    private bool _canShoot;
    private Camera _camera;
    private PlayerMovement _playerMovement;
    private WeaponHolder _weaponHolder;
    public TMP_Text[] _currentBullet;
    public TMP_Text[] _leftOverBullet;
    private string _isAiming = "IsAiming";
    void Start()
    {
        _gunOffset = transform.GetChild(1);
        _spriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(_spriteRenderer.sprite);
        _crosshairController = GameObject.Find("Crosshair").GetComponent<CrosshairController>();
        _camera = Camera.main;
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _meeleAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
    }
    void Update()
    {
        if(_weaponHolder.GetCurrentWeapon() != null){
            _gun = _weaponHolder.GetCurrentWeapon();
        }
        UpdateBulletText();
        if(_playerMovement._isAiming){
            ChangeSpriteBasedOnMouse();
            if(_continousAttack && !_gun._isReloading && _canShoot){
                _crosshairController.ExpandCrosshair();
                SoundManager.instance.PlaySFX(_weaponHolder.GetCurrentWeapon().AudioAttack);
                _weaponHolder.GetCurrentWeapon().Fire();
                _crosshairController.ResetCrosshair();
                timer = _weaponHolder.GetCurrentWeapon()._fireRate;
                _canShoot = false;
            }

            if(!_canShoot){
                timer -= Time.deltaTime;
                if(timer < 0) _canShoot = true;
            }
        }else{
        _animator.SetBool(_isAiming, false);
        }
    }
    void OnAttack(InputValue input){
        if(!_playerMovement._isAiming) return;
        _continousAttack = input.isPressed;
    }
    void OnAim(InputValue input){
        _playerMovement._isAiming = !_playerMovement._isAiming;
        _playerMovement.SetAiming(_playerMovement._isAiming);
    }

    public void FireBullet(GameObject _bulletPrefabs){
        if(_weaponHolder.GetCurrentWeapon()._currentAmmo <= 0) return;
        
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(_bulletPrefabs, _gunOffset.position, Quaternion.Euler(0, 0, angle));
        bullet.transform.up = direction;
        Rigidbody2D _rbBullet = bullet.GetComponent<Rigidbody2D>();
        _rbBullet.linearVelocity = _weaponHolder.GetCurrentWeapon()._bulletSpeed * direction;
    }
    public void UpdateBulletText()
    {
        if(_gun == null) return;
        if(_gun._currentAmmo == 0){
            for(int i = 0; i < _currentBullet.Length; i++){
                _currentBullet[i].color = Color.red;
                _leftOverBullet[i].color = Color.red;
            }
        }else{
            for(int i = 0; i < _currentBullet.Length; i++){
                _currentBullet[i].color = Color.white;
                _leftOverBullet[i].color = Color.white;
            }
        }
        for(int i = 0; i < _currentBullet.Length; i++){
            _currentBullet[i].text = $"{_gun._currentAmmo} i";
            _leftOverBullet[i].text = $"{_gun._leftOverAmmo}";
        }
    }

    private void ChangeSpriteBasedOnMouse(){
        _animator.SetBool(_isAiming, true);
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.y);
    }

    private void OnMeeleAttack(InputValue input){
        if(input.isPressed && !_playerMovement._isMeeleAttack && !_playerMovement._isAiming){
            StartCoroutine(CoroutineMeeleAttack(MeeleDuration));
        }
    }
    private IEnumerator CoroutineMeeleAttack(float meeleDuration){
        _playerMovement._isMeeleAttack = true;
        _meeleAnimator.SetBool("IsMeeleAttack", _playerMovement._isMeeleAttack);
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _meeleAnimator.SetFloat("Horizontal", direction.x);
        _meeleAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector2 attackCenter = (Vector2) transform.position + direction * MeeleRange;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, MeeleSize, angle, _enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Enemy"))
            {
                HealthController healthController = enemy.GetComponent<HealthController>();
                healthController.TakeDamage(MeeleDamage);
                SoundManager.instance.PlaySFX(SoundManager.instance.HitMeele);
            }
        }
        SoundManager.instance.PlaySFX(SoundManager.instance.AttackMeele);
        yield return new WaitForSeconds(meeleDuration);
        _playerMovement._isMeeleAttack = false;
        _meeleAnimator.SetBool("IsMeeleAttack", _playerMovement._isMeeleAttack);
    }

    private void OnDrawGizmos()
    {
        if (_camera == null) return;

        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePos - transform.position).normalized;
        Vector2 attackCenter = (Vector2)transform.position + attackDirection * MeeleRange;

        float attackAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attackCenter, Quaternion.Euler(0, 0, attackAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, MeeleSize);
    }
}
