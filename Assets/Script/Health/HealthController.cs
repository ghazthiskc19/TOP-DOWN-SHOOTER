using System;
using UnityEngine;
using UnityEngine.Events;
public class HealthController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    public float _maxHealth {get; private set;}
    private Animator _animator;
    void Awake()
    {
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        UpdateAllHealthBarsPlayer((a) => a.UpadteHealthBar(this));
        _maxHealth = _currentHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void SetCurrentHealth(float value){
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) =>{
                _currentHealth = _maxHealth;
                a.UpadteHealthBar(this);
            });
        }else{
            _currentHealth = value;
        }
    }
    public float RemainingHealth
    {
        get 
        {
            return _currentHealth / _maxHealth;
        }
    }
    public bool IsInvicible { get; set; }
    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealthChanged;
    public bool gotDOT;
    float elapsedTime = 0f;
    public float duration = 3f;
    public float timer = 0;
    private void Update()
    {
        if(timer < SoundManager.instance.HitPlayer.length){
            timer += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }else{
            timer = 0;
            if (gotDOT){
                TakeDamage(3 * 10);
                if (elapsedTime >= duration){
                    StopDOT();
                }
            }
        }
    }
    public void TakeDamage(float damageAmount){
        if(_currentHealth == 0){
            return;
        }

        if(_currentHealth < 0){
            _currentHealth = 0;
        }

        if(IsInvicible){
            IsInvicible = false;
            return;
        }
        _currentHealth -= damageAmount; 
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) => a.UpadteHealthBar(this));
            SoundManager.instance.PlaySFX(SoundManager.instance.HitPlayer);
        }

        if(GetComponent<EnemyAnimControl>()){
            SoundManager.instance.PlaySFX(SoundManager.instance.HitEnemy);
        }
        OnHealthChanged.Invoke();
        if(_currentHealth <= 0 ){
            OnDied.Invoke();    
            if(_animator != null) _animator.SetBool("IsDead", true);
        }else{
            OnDamaged.Invoke();
        }
    }

    public void AddHealth(float amountToAdd){
        if(_currentHealth == _maxHealth){
            return;
        }
        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
        if(_currentHealth >= _maxHealth){
            _currentHealth = _maxHealth;
        }
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) => a.UpadteHealthBar(this));
        }
    }
   public void ResetHealth()
    {
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) =>  {
                _currentHealth = _maxHealth;
                a.UpadteHealthBar(this);
            });
        }
    }
    private void UpdateAllHealthBarsPlayer(Action<HealthBarUI> innerFunc)
    {
        HealthBarUI[] healthBarUI = FindObjectsByType<HealthBarUI>(FindObjectsSortMode.None);
        foreach (HealthBarUI healthbar in healthBarUI){
            if(healthbar.gameObject.name == "Health Bar" && healthbar.gameObject.CompareTag("Healthbar player")){
                    innerFunc?.Invoke(healthbar);
            }
        }
    }
    public void StartDOT(float dotDuration)
    {
        if (!gotDOT)
        {
            gotDOT = true;
            duration = dotDuration;
            elapsedTime = 0f;
        }
    }

    public void StopDOT()
    {
        gotDOT = false;
        elapsedTime = 0f;
    }
}
