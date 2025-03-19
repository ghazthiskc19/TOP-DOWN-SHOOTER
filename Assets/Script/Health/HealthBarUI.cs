using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForeground;
    [SerializeField] private AnimationCurve curve;
    private float _currentHealth;
    public void UpadteHealthBar(HealthController healthController){
        StartCoroutine(UpdateHealthBarSmooth(healthController));
    }
    private IEnumerator UpdateHealthBarSmooth(HealthController healthController){

        _currentHealth = _healthBarForeground.fillAmount;
        float t = 0;
        int timeAnimation = 1;
        while(t < 1){
            t += Time.deltaTime;
            float elapsedTime = t / timeAnimation;
            _healthBarForeground.fillAmount = Mathf.Lerp(_currentHealth ,healthController.RemainingHealth, curve.Evaluate(elapsedTime));
            yield return null;
        }
    }
    void Start()
    {
        HealthController healthController = GetComponentInChildren<HealthController>();
        if(healthController != null) _healthBarForeground.fillAmount = healthController.RemainingHealth;
    }
}
