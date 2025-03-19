using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class HealthController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    private Animator _animator;
    void Awake()
    {
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
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
            HealthBarUI[] healthBarUI = FindObjectsByType<HealthBarUI>(FindObjectsSortMode.None);
            foreach (HealthBarUI healthbar in healthBarUI){
                if(healthbar.gameObject.name == "Health Bar"){
                    healthbar.UpadteHealthBar(this);
                }
            }
        }
        OnHealthChanged.Invoke();
        if(_currentHealth <= 0 ){
            OnDied.Invoke();
            _animator.SetBool("IsDead", true);
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
            HealthBarUI[] healthBarUI = FindObjectsByType<HealthBarUI>(FindObjectsSortMode.None);
            foreach (HealthBarUI healthbar in healthBarUI){
                if(healthbar.gameObject.name == "Health Bar"){
                    healthbar.UpadteHealthBar(this);
                }
            }
        }
        OnHealthChanged.Invoke();
        if(_currentHealth >= _maxHealth){
            _currentHealth = _maxHealth;
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
