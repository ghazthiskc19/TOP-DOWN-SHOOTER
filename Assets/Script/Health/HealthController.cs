using UnityEngine;
using UnityEngine.Events;
public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private float _maxHealth;
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
    public void TakeDamage(float damageAmount){
        if(_currentHealth == 0){
            return;
        }

        if(_currentHealth < 0){
            _currentHealth = 0;
        }

        if(IsInvicible){
            return;
        }
        _currentHealth -= damageAmount; 
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
        OnHealthChanged.Invoke();
        if(_currentHealth >= _maxHealth){
            _currentHealth = _maxHealth;
        }
    }
}
