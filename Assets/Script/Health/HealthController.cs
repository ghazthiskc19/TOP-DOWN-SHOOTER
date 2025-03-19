using System;
using UnityEngine;
using UnityEngine.Events;
public class HealthController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    private Animator _animator;
    void Awake()
    {
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        UpdateAllHealthBarsPlayer((a) => a.UpadteHealthBar(this));
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void SetCurrentHealth(float value){
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) =>  {
                _currentHealth = value;
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

    private void Update()
    {
        if (gotDOT){
            TakeDamage(3 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration){
                StopDOT();
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
        if(GetComponent<PlayerMovement>()){
            UpdateAllHealthBarsPlayer((a) => a.UpadteHealthBar(this));
        }
        OnHealthChanged.Invoke();
        if(_currentHealth >= _maxHealth){
            _currentHealth = _maxHealth;
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
