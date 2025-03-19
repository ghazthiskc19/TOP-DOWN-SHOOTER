using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _changeDirectionCooldown;
    [SerializeField]
    private float _screenBorder;
    private Camera _camera;
    private Vector2 _targetDirection;
    private PlayerAwarenessController _playerAwarenessController;
    private Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection =  transform.up;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        UpdateVelocity();
    }

    void UpdateTargetDirection(){
        ChangeRandomPosition();
        ChangeDirectionWhenNearPlayer();
        TurnAroundWhenGoOutside();
    }

    void ChangeRandomPosition()
    {
        _changeDirectionCooldown -= Time.deltaTime;
        if(_changeDirectionCooldown <= 0){  
            float RandomAngle = Random.Range(-90f, 90f);
            Quaternion NewRotation = Quaternion.AngleAxis(RandomAngle, transform.forward);
            _targetDirection = NewRotation * _targetDirection;
            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void TurnAroundWhenGoOutside(){
        Vector3 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if(
            (screenPosition.x < _screenBorder && _targetDirection.x < 0 ) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0 )
        )
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }

        if(
            (screenPosition.y < _screenBorder && _targetDirection.y < 0 ) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0 )
        )
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }
    void ChangeDirectionWhenNearPlayer()
    {
        if(_playerAwarenessController.AwareOfPlayer){
        _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    void RotateTowardsTarget()
    {

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        // Biar dia gak tiba tiba sampe di rotasi yang diinginkan (ada transisi dari rotasi awal ke targetRotasi)
        // Dengan waktu yaitu _rotationSpeed * Time.deltaTime
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rb.SetRotation(rotation);

        // Dia chase enemy + ada efek fisika dari rigidBody2D nya, kalau pake vector
        // float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg - 90;
        // Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        // Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        // _rb.SetRotation(newRotation);
    }

    void UpdateVelocity()
    {
        _rb.linearVelocity = transform.up * _moveSpeed;
    }
}
