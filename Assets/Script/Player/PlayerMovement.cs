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
    private WeaponPickup _weaponPickup;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Vector2 _smoothedMovement;
    private Vector2 _movementInputSmoothVelocity;
    private Animator _animator;
    public bool _isAiming = false;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _LastHorizontal = "LastHorizontal";
    private const string _LastVertical = "LastVertical";
    private Gun _gun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _crosshairController = FindAnyObjectByType<CrosshairController>();
        _gun = GetComponentInChildren<Gun>();
    }

    void Update()
    {
        _animator.SetFloat(_horizontal, _moveInput.x);
        _animator.SetFloat(_vertical, _moveInput.y);
        if(_moveInput != Vector2.zero){
            _animator.SetFloat(_LastHorizontal, _moveInput.x);
            _animator.SetFloat(_LastVertical, _moveInput.y);
        }

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
        _rb.linearVelocity = _smoothedMovement * _moveSpeed;
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
        if(!_isAiming){
            _moveInput = input.Get<Vector2>();
        }
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
            _moveInput = Vector2.zero;
        }
    }

    private void OnCollect(InputValue input){
        if(input.isPressed){
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach(Collider2D hit in hits){
                WeaponPickup weaponPickup = hit.GetComponent<WeaponPickup>();
                if(weaponPickup != null){
                    TryPickupWeapon(weaponPickup);
                }
            }
        }
    }
    public void TryPickupWeapon(WeaponPickup weaponPickup)
    {
        weaponPickup.IsPickup = true;
    }
}

