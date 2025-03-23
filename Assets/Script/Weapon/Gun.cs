using System.Collections;
using TMPro;
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
    public AudioClip AudioAttack;
    private AudioSource reloadAudioSource;
    private PlayerShoot _playerShoot;
    private PlayerMovement _playerMovement; 
    private bool isReloading = false;
    public bool _isReloading => isReloading;
    private bool _pressReloadKey;
    private float timer = 0f;
    private  TMP_Text _reloadText;
    private UnityEngine.UI.Image _reloadImageFill;
    private CanvasGroup _reloadUI;
    private void Awake()
    {
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _playerShoot = GameObject.Find("Player").GetComponent<PlayerShoot>();
        _reloadUI = GameObject.Find("Reload UI").GetComponent<CanvasGroup>();
        _reloadText = _reloadUI.transform.GetChild(2).GetComponent<TMP_Text>();
        _reloadImageFill = _reloadUI.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
    }
    private void Start()
    {
        if(_reloadUI != null){
            _reloadUI.alpha = 0f;
        }
    }

    public void OnReload(InputValue input)
    {
        if(input.isPressed && _currentAmmo < _magazineSize)
        {
            _pressReloadKey = true; 
        }
    }

    public void OnCollect(InputValue input){
        if(input.isPressed){
            _reloadUI.alpha = 1f;
        }
    }
    private void Update()
    {
        if(isReloading && !_playerMovement._isAiming){
            _reloadUI.alpha = 0f;
            CancelReload();
            return;
        }

        if (!isReloading && (_pressReloadKey || (_currentAmmo == 0 && _playerMovement._isAiming)))
        {
            isReloading = true;
            timer = 0f;
            _reloadUI.alpha = 1f;
        }
        if(isReloading){
            if(!SoundManager.instance.reloadSource.isPlaying){
                if(_weaponName == "Sniper")
                {
                    if(_currentAmmo == 0){
                        reloadAudioSource = SoundManager.instance.PlayReloadSFX(SoundManager.instance.SniperReload);
                    }else{
                        reloadAudioSource = SoundManager.instance.PlayReloadSFX(SoundManager.instance.SniperReload);
                    }
                }else
                {
                    reloadAudioSource = SoundManager.instance.PlayReloadSFX(SoundManager.instance.revolverReload);
                }
            }
            
            timer += Time.deltaTime;
            _reloadText.text =  (_reloadTime - timer).ToString("F1") + "s";
            _reloadImageFill.fillAmount = timer / _reloadTime;
            if(timer >= _reloadTime){
                CompleteReload();
                _reloadUI.alpha = 0f;
            }
        }
    }

    private IEnumerator PlayReloadWithDelay(AudioClip audio, float time)
    {
        yield return new WaitForSeconds(time);
        reloadAudioSource = SoundManager.instance.PlayReloadSFX(audio);
    }

    public void Fire()
    {
        if(_currentAmmo > 0 && !isReloading)
        {
            _playerShoot.FireBullet(_bullet);
            _currentAmmo--;
        }
    }

    private void CompleteReload()
    {
        int ammoNeeded = _magazineSize - _currentAmmo;
        if(_leftOverAmmo >= ammoNeeded)
        {
            _currentAmmo += ammoNeeded;
            _leftOverAmmo -= ammoNeeded;
        }
        else 
        {
            _currentAmmo += _leftOverAmmo;
            _leftOverAmmo = 0;
        }
        isReloading = false;
        _pressReloadKey = false;
        timer = 0f;
        _playerShoot.UpdateBulletText();
    }

    private void CancelReload()
    {   
        isReloading = false;
        timer = 0f;
        _pressReloadKey = false;
        SoundManager.instance.StopReloadSFX();
        Debug.Log("Reload dibatalkan karena tidak lagi aim.");
    }    
}
