using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public HealthController HealthController {get ; set;}
    public WeaponHolder WeaponHolder {get; set;}
    public SpawnManager SpawnManager {get; set;}
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
    private bool _isPlayingWalkAudio = false;
    private Gun _gun;
    void Awake()
    {
        if(instance == null) instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _crosshairController = FindAnyObjectByType<CrosshairController>();
        HealthController = GetComponent<HealthController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            LoadGame();
        }
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

    public  void LoadGame()
    {
        SaveSystem.Load();
    }

    public void SaveGame()
    {
        SaveSystem.Save();
    }
    private void SetPlayerVelocity()
    {
        _smoothedMovement = Vector2.SmoothDamp(
            _smoothedMovement,
            _moveInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );
        if(!_isAiming && _moveInput.magnitude > 0.1f && !_isMeeleAttack && !_isPlayingWalkAudio){
            StartCoroutine(WalkAudio(SoundManager.instance.JalanVer1));
        }
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

    private IEnumerator WalkAudio(AudioClip audio){
        _isPlayingWalkAudio = true;
        SoundManager.instance.PlaySFX(audio);
        yield return new WaitForSeconds(audio.length);
        _isPlayingWalkAudio = false;
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

    public void LastPositoin()
    {
        transform.position = RespawnController.instance.lastCheckpointPos;
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(-10.7f, -0.4f);
    }
    public void ResetPlayerSprite()
    {
        _animator.SetBool("IsDead", false);
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

    public void Save(ref PlayerSaveData data)
    {
        data.playerPos = RespawnController.instance.lastCheckpointPos;
        data.currentHealth = HealthController._maxHealth;
        Debug.Log(data.currentHealth);
    }
    public void Load(PlayerSaveData data)
    {
        Debug.Log(data.currentHealth);
        transform.position = data.playerPos;
        HealthController.SetCurrentHealth(data.currentHealth);
    }
}
[System.Serializable]
public struct PlayerSaveData{
    public Vector3 playerPos;
    public float currentHealth;

}