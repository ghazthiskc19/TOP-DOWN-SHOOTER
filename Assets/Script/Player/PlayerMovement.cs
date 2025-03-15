using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;
    [SerializeField] private CrosshairController _crosshairController;
    private Camera _camera;
    private Rigidbody2D _rb;
    public Vector2 _moveInput {get; private set;}
    private Vector2 _smoothedMovement;
    private Vector2 _movementInputSmoothVelocity;
    private Animator _animator;
    public bool _isAiming = false;
    public  bool _isMeeleAttack = false;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _LastHorizontal = "LastHorizontal";
    private const string _LastVertical = "LastVertical";
    private Gun _gun;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _crosshairController = FindAnyObjectByType<CrosshairController>();
    }

    void Update()
    {
        // float velX = _rb.linearVelocityX;
        // float velY = _rb.linearVelocityY;
        // if (Mathf.Abs(velX) < 0.1f) velX = 0f;
        // if (Mathf.Abs(velY) < 0.1f) velY = 0f;
        if(!_isAiming){
            _animator.SetFloat(_vertical, _moveInput.y);
            _animator.SetFloat(_horizontal, _moveInput.x);
        }

        if(_moveInput != Vector2.zero){
            _animator.SetFloat(_LastHorizontal, _moveInput.x);
            _animator.SetFloat(_LastVertical, _moveInput.y);
        }
        _gun = GetComponentInChildren<Gun>();
    }
    void FixedUpdate()
    {
        // Smoothing movement
        SetPlayerVelocity();      
    }

    private void SetPlayerVelocity()
    {
        _smoothedMovement = Vector2.SmoothDamp(
            _smoothedMovement,
            _moveInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );
        // Perubahan rb mending simpen disini
        if(!_isAiming && !_isMeeleAttack){
            _rb.linearVelocity = _smoothedMovement * _moveSpeed;
        }else{
            _rb.linearVelocity = Vector2.zero;
            _animator.SetFloat(_vertical, 0);
            _animator.SetFloat(_horizontal, 0);
        }

        HandlePlayerWhenGoingOutside();
    }

    private void HandlePlayerWhenGoingOutside(){
        Vector3 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if(
            (screenPosition.x < _screenBorder && _rb.linearVelocity.x < 0 ) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _rb.linearVelocity.x > 0 )
        )
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }

        if(
            (screenPosition.y < _screenBorder && _rb.linearVelocity.y < 0 ) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _rb.linearVelocity.y > 0 )
        )
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
        }
    }
    private void OnMove(InputValue input){
        _moveInput = input.Get<Vector2>();
    }

    private void OnReload(InputValue input)
    {
        _gun.OnReload(input);
    }
    public void SetAiming(bool isAiming){
        _isAiming = isAiming;
        _crosshairController.SetCrosshairVisibility(true);
        if(_isAiming){
            _rb.linearVelocity = Vector2.zero;
            _animator.SetFloat(_vertical, 0);
            _animator.SetFloat(_horizontal, 0);
        }
    }

    private void OnCollect(InputValue input){
        bool _hasPickup = false;
        if(input.isPressed && !_hasPickup && !_gun._isReloading){
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f, ~0);
            foreach(Collider2D hit in hits){
                if(hit.GetComponent<WeaponPickup>()){
                    WeaponPickup weaponPickup = hit.GetComponent<WeaponPickup>();
                    if(weaponPickup != null && !hit.transform.IsChildOf(transform)){
                        TryPickupWeapon(weaponPickup);
                        _hasPickup = true;
                    }
                }
            }
        }
    }
    public void TryPickupWeapon(WeaponPickup weaponPickup)
    {
        weaponPickup.IsPickup = true;
    }
}

