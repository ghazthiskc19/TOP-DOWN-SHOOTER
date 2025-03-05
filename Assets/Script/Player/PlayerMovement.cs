using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _screenBorder;
    private Camera _camera;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Vector2 _smoothedMovement;
    private Vector2 _movementInputSmoothVelocity;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 _mousePosition = Input.mousePosition;
        _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        Vector2 direction = new Vector2(_mousePosition.x - transform.position.x, _mousePosition.y - transform.position.y);
        transform.up = direction;

    }
    void FixedUpdate()
    {
        // Smoothing movement
        SetPlayerVelocity();
        // RotateInDirectionOfInput();
        // if(_rb.linearVelocity.magnitude > 0.1f)
        // {
        //     _animator.SetBool("IsRunning", true);
        // }else
        // {
        //     _animator.SetBool("IsRunning", false);
        // }

        if(_moveInput != Vector2.zero){
            _animator.SetBool("IsRunning", true);
        }else
        {
            _animator.SetBool("IsRunning", false);
        }        
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
    private void RotateInDirectionOfInput(){
        if(_moveInput != Vector2.zero){
            // Vector3 direction = new Vector3(_smoothedMovement.x, _smoothedMovement.y, 0);

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovement);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rb.SetRotation(rotation);

            // float targetAngle = Mathf.Atan2(_smoothedMovement.y, _smoothedMovement.x) * Mathf.Rad2Deg;
            // _rb.rotation = Mathf.LerpAngle(_rb.rotation, targetAngle, _rotationSpeed * Time.deltaTime);
        }

    }

    // Update is called once per frame
    private void OnMove(InputValue input){
        _moveInput = input.Get<Vector2>();
    }
}
